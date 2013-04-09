namespace ValtechLondonLab.Modules
{
    using System.Net.Http;

    using ImageResizer;

    using Nancy;

    using ValtechLondonLab.DAL;
    using ValtechLondonLab.DAL.Entities;

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

            this.Get["/hotel/{hotelid}"] = this.GetHotel;

            this.Get["/hotel/{hotelid}/image/width/{width}/height/{height}"] = this.GetHotelImage;
        }

        private ITravelOffersRepository TravelOffersRepository { get; set; }

        private HttpClient ImageDownloadClient { get; set; }

        public object GetHotel(dynamic parameters)
        {
            Hotel hotel = this.TravelOffersRepository.GetHotel(parameters.hotelid);

            if (hotel == null)
            {
                return new Response { StatusCode = HttpStatusCode.NotFound };
            }

            var hotelModel =
                new
                    {
                        html = hotel.Html,
                        grade = hotel.Grade,
                        name = hotel.Name,
                        url = hotel.Url,
                        hasImage = !string.IsNullOrEmpty(hotel.ImageUrl)
                    };

            return hotelModel;
        }

        public Response GetHotelImage(dynamic parameters)
        {
            var hotelId = parameters.hotelid;
            var imageWidth = parameters.width;
            var imageHeight = parameters.height;

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