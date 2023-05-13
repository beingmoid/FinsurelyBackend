


using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.CatalogDb;

namespace PanoramaBackend.Data.CatalogDb.Repos
{
    public class CardInfoRepo : EFRepository<CardInfo, int>, ICardInfoRepo
    {
        public CardInfoRepo(CatalogDbContext requestScope) : base(requestScope)
        {

        }
    }
    public interface ICardInfoRepo : IEFRepository<CardInfo, int>
    {

    }
}
