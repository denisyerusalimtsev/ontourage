﻿using Ontourage.Core.Entities;
using System.Collections.Generic;

namespace Ontourage.Core.Interfaces
{
    public interface IPaymentChecksRepository
    {
        List<PaymentCheck> GetAllPaymentChecks();

        PaymentCheck ViewDetails(int id);

        PaymentCheck GetPaymentCheckById(int id);

        void AddPaymentCheck(PaymentCheck paymentCheck);
    }
}
