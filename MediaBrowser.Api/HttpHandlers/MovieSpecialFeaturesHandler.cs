﻿using MediaBrowser.Common.Net.Handlers;
using MediaBrowser.Model.DTO;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Entities.Movies;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MediaBrowser.Api.HttpHandlers
{
    /// <summary>
    /// This handler retrieves special features for movies
    /// </summary>
    [Export(typeof(BaseHandler))]
    public class MovieSpecialFeaturesHandler : BaseSerializationHandler<DTOBaseItem[]>
    {
        public override bool HandlesRequest(HttpListenerRequest request)
        {
            return ApiService.IsApiUrlMatch("MovieSpecialFeatures", request);
        }

        protected override Task<DTOBaseItem[]> GetObjectToSerialize()
        {
            User user = ApiService.GetUserById(QueryString["userid"], true);

            Movie movie = ApiService.GetItemById(ItemId) as Movie;

            // If none
            if (movie.SpecialFeatures == null)
            {
                return Task.FromResult<DTOBaseItem[]>(new DTOBaseItem[] { });
            }

            return Task.WhenAll<DTOBaseItem>(movie.SpecialFeatures.Select(i =>
            {
                return ApiService.GetDTOBaseItem(i, user, includeChildren: false, includePeople: true);
            }));
        }

        protected string ItemId
        {
            get
            {
                return QueryString["id"];
            }
        }
    }
}
