using ExpenseTracker.Application.Hubs;
using ExpenseTracker.Application.Mappings;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Requests.Wallet;
using ExpenseTracker.Application.Requests.WalletShare;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Application.Stores.Interfaces;
using ExpenseTracker.Application.ViewModels.Wallet;
using ExpenseTracker.Application.ViewModels.WalletShare;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker.Application.Stores;

internal sealed class WalletStore : IWalletStore
{
    private readonly ICommonRepository _repository;
    private readonly IEmailService _emailService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public WalletStore(ICommonRepository repository, IEmailService emailService, IHubContext<NotificationHub> hubContext)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public List<WalletViewModel> GetAll(GetWalletsRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wallets = _repository.Wallets.GetAll(request.UserId, request.Search);

        var viewModels = wallets
            .Select(x => x.ToViewModel())
            .OrderByDescending(x => x.Id)
            .ToList();

        return viewModels;
    }

    public WalletViewModel GetById(WalletRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wallet = _repository.Wallets.GetById(request.Id, request.UserId);

        var viewModel = wallet.ToViewModel();

        return viewModel;
    }

    public WalletViewModel Create(CreateWalletRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = request.ToEntity();

        var createdWallet = _repository.Wallets.Create(entity);
        _repository.SaveChanges();

        createdWallet.Owner = _repository.Users.GetById(request.UserId);
        var viewModel = createdWallet.ToViewModel();

        return viewModel;
    }

    public WalletViewModel CreateDefault(Guid userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var request = GetDefaultWallet(userId);

        return Create(request);
    }

    public WalletViewModel Update(UpdateWalletRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = request.ToUpdateEntity();

        if (entity.IsMain)
        {
            var main = _repository.Wallets.GetMain(request.UserId);
            ChangeMainToSecondary(main);
        }

        _repository.Wallets.Update(entity);
        _repository.SaveChanges();

        var viewModel = entity.ToViewModel();

        return viewModel;
    }

    public void Delete(WalletRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        _repository.Wallets.Delete(request.Id, request.UserId);
        _repository.SaveChanges();
    }

    public WalletShareViewModel GetWalletShareById(WalletShareRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wallet = _repository.WalletShares.GetById(request.Id);

        if (wallet is null)
        {
            throw new EntityNotFoundException($"Wallet share with id: {request.Id} is not found.");
        }

        if (wallet.UserId == request.UserId) 
        {
            // throw exception
        }

        wallet.Wallet = _repository.Wallets.GetById(wallet.WalletId);
        wallet.User = _repository.Users.GetById(wallet.UserId);
        wallet.Wallet.Owner = _repository.Users.GetById(wallet.Wallet.OwnerId);

        var viewModel = wallet.ToViewModel();

        return viewModel;
    }

    public async void Share(CreateWalletShareRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wallet = GetAndValidateWallet(request);
        await _hubContext.Clients.All
            .SendAsync("IncrementNotificationsCount", wallet.Id);

        foreach (var userEmail in request.UsersToShare)
        {
            var user = _repository.Users.GetByEmail(userEmail);

            if (user is null)
            {
                var message = new EmailMessage(userEmail, "there", "Collaboration Invitation", null);
                _emailService.SendWalletInvitation(message);
            }
            else
            {
                var share = new WalletShare
                {
                    User = null!,
                    UserId = user.Id,
                    Wallet = null!,
                    WalletId = wallet.Id,
                    AccessType = WalletAccessType.ReadAndWrite,
                    Date = DateTime.UtcNow,
                    IsAccepted = false,
                };

                _repository.WalletShares.Create(share);
                _repository.SaveChanges();

                var notification = new Notification
                {
                    IsRead = false,
                    Title = "Wallet Collaboration Invitation",
                    Body = $"{user.UserName} invites you to collaborate on Wallet: {wallet.Name}.",
                    RedirectUrl = $"/wallets/shares/{share.Id}",
                    User = user,
                };

                _repository.Notifications.Create(notification);
                _repository.SaveChanges();
            }
        }
    }

    private static CreateWalletRequest GetDefaultWallet(Guid userId) => new(
        UserId: userId,
        Name: "Default Wallet",
        Description: "This is default Wallet generated by the system. You can update or delete it.",
        Balance: 0);

    private static void ChangeMainToSecondary(Wallet? wallet)
    {
        if (wallet is not null)
        {
            wallet.IsMain = false;
        }
    }

    private Wallet GetAndValidateWallet(CreateWalletShareRequest request)
    {
        var wallet = _repository.Wallets.GetById(request.WalletId);
        wallet.Owner = _repository.Users.GetById(wallet.OwnerId);

        if (wallet is null)
        {
            throw new EntityNotFoundException($"Wallet with id: {request.WalletId} is not found.");
        }

        if (wallet.OwnerId != request.UserId)
        {
            throw new ShareNotAllowedException("Only owner can share wallet.");
        }

        return wallet;
    }
}
