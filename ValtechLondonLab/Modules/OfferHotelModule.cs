namespace ValtechLondonLab.Modules
{
    using System.Net.Http;

    using ImageResizer;

    using Nancy;

    using ValtechLondonLab.DAL;

    public class OfferHotelModule : NancyModule
    {
        public OfferHotelModule(ITravelOffersRepository offersRepository)
            : this(offersRepository, new HttpClient())
        {
        }

        public OfferHotelModule(ITravelOffersRepository offersRepository, HttpClient imageDownloadClient)
            : base("/offer")
        {
            this.TravelOffersRepository = offersRepository;

            this.ImageDownloadClient = imageDownloadClient;

            this.Get["/hotel/{hotelid}"] = parameters =>
                { 
                    var hotel = offersRepository.GetHotel(parameters.hotelid);

                    if (hotel == null)
                    {
                        return new Response { StatusCode = HttpStatusCode.NotFound };
                    }

                    return hotel;
                };

            this.Get["/hotel/{hotelid}/image/width/{width}/height/{height}"] =
                parameters => this.GetHotelImage(parameters.hotelid, parameters.width, parameters.height);
        }

        private ITravelOffersRepository TravelOffersRepository { get; set; }

        private HttpClient ImageDownloadClient { get; set; }

        public Response GetHotelImage(string hotelId, string imageWidth, string imageHeight)
        {
            var response = new Response();

            var hotel = this.TravelOffersRepository.GetHotel(hotelId);

            if (hotel == null || string.IsNullOrEmpty(hotel.ImageUrl))
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            var imageResponse = this.ImageDownloadClient.GetAsync(hotel.ImageUrl).Result;

            if (!imageResponse.IsSuccessStatusCode)
            {
                response.StatusCode = (HttpStatusCode)imageResponse.StatusCode;
                return response;
            }

            var resizeSettings = string.Format("width={0}&height={1}&crop=auto", imageWidth, imageHeight);
            response.ContentType = imageResponse.Content.Headers.ContentType.MediaType;

            response.Contents = stream =>
                {
                    var imageStream = imageResponse.Content.ReadAsStreamAsync().Result;

                    var i = new ImageJob(imageStream, stream, new ResizeSettings(resizeSettings));
                    i.Build();
                };

            return response;
        }
    }
}