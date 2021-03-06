﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using PosApp.Domain;
using PosApp.MockService;
using PosApp.Repositories;

namespace PosApp.Services
{
    public class PromotionService
    {
        readonly PromotionRespository m_promotoionRepository;
        readonly IProductRepository m_productRespository;
        readonly IDateTime m_dateTime;

        public PromotionService(PromotionRespository promotoionRepository,
            IProductRepository productRespository, IDateTime dateTime)
        {
            m_promotoionRepository = promotoionRepository;
            m_productRespository = productRespository;
            m_dateTime = dateTime;
        }

        public List<string> GetBarcodes(string type)
        {
            return m_promotoionRepository.GetBarcodesByType(type)
                .Select(p => p.Barcode).ToList();
        } 
       
        public void AddBarcode(string type,string[]barcodes)
        {
            Validate(barcodes);
            List<string> typeBarcodes = GetBarcodes(type);
            List<Promotion> promotions = barcodes.Where(b => typeBarcodes.Contains(b) == false)
                .Select(nb => new Promotion {Type = type, Barcode = nb}).ToList();
            m_promotoionRepository.Save(promotions);
        }

        public void DeleteBarcode(string type, string[] barcodes)
        {
            List<Promotion> promotions = m_promotoionRepository.GetByBarcode(barcodes)
                .Where(p => p.Type == type)
                .ToList();
            m_promotoionRepository.Delete(promotions);
        }

        void Validate(string[] barcodes)
        {
            string[] uniqueBarcodes = barcodes.Distinct().ToArray();
            if (m_productRespository.CountByBarcodes(uniqueBarcodes) != uniqueBarcodes.Length)
            {
                throw new ArgumentException("Some of the products cannot be found.");
            }
        }

        public Receipt BuildPromotion(Receipt receipt)
        {
            return m_promotoionRepository.GetAllTypes()
                .Select(type => CreatePromotion(type))
                .Aggregate(receipt,(r, promotion) => promotion.GetPromotionReceipt(r));
        }

        ICaculatePromotions CreatePromotion(string type)
        {
            if (type.Equals("BUY_TWO_GET_ONE"))
                return new BuyTwoGetOne(m_promotoionRepository);
            if (type.Equals("BUY_HUNDRED_CUT_FIFTY"))
                return new HundredCutFifty(m_promotoionRepository,m_dateTime);
            throw new NotSupportedException("not supported type");
        }
    }
}