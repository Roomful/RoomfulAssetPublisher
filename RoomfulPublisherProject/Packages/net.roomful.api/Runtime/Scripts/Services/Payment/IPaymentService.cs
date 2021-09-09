using System;
using System.Collections.Generic;
using net.roomful.api.sa;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.payment
{
    public interface IPaymentService
    {
        void Login();
        void Show(Action onHideCallback = null);
        void GetTokenActionList(Action<IReadOnlyList<IFAQTokenActionTemplate>> action);
        Dictionary<TransactionFilterType, List<ITransactionActionTemplate>> Content { get; }
        SA_iSafeEvent CoinsChanged { get; }
        long Coins { get; }
        void ShowInsufficientFunds(string msg);
        void Donate(IUserTemplateSimple receiver, int donationValue, Action action);
    }
}
