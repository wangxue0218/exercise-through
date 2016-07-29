using System;
using PosApp.Domain;

namespace PosApp.Services
{
    public interface ICaculatePromotions
    {
        Receipt GetPromotionReceipt(Receipt receipt);
    }
}