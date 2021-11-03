using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCargoMobility.Models;
using Xamarin.Forms;

namespace VCargoMobility.ViewModels
{
public    class SP_ViewModel:BaseViewModel
    {

        private bool _isRefreshing;
        private string _RefNo;
        private string _Type;
        private string itemId;
        private string _mwabno;

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                //  LoadItemId(value);
            }
        }

        bool isBusy;
        public bool IsVisibleHere
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }

        }

        public string RefNo
        {
            get => _RefNo;
            set => SetProperty(ref _RefNo, value);
        }

        public string StatusType
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                // LoadItemId(value);
            }
        }

        public string MWABNo
        {
            //get => _ItemCode;
            //set => SetProperty(ref _ItemCode, value);

            get { return _mwabno; }
            set { SetProperty(ref _mwabno, value); }

        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ObservableCollection<Carrier> CarrierList { get; }
        public ObservableCollection<Delivered> DeliveredDocuments { get; }
        public ObservableCollection<Pending> PendingDocuments { get; }
        public ObservableCollection<Transfer> TransferDocuments { get; }
        public ObservableCollection<Success> SuccessDocuments { get; }
        public ObservableCollection<Failed> FailedDocuments { get; }
        public ObservableCollection<Summary> SummaryDocuments { get; }




        public SP_ViewModel()
        {
            CarrierList = new ObservableCollection<Carrier>();
            // MCA collection
            
            DeliveredDocuments = new ObservableCollection<Delivered>();
            SuccessDocuments = new ObservableCollection<Success>();
            TransferDocuments = new ObservableCollection<Transfer>() ;
            PendingDocuments = new ObservableCollection<Pending>();
            FailedDocuments = new ObservableCollection<Failed>();
            SummaryDocuments = new ObservableCollection<Summary>();
            // Command
            ScanCommand = new Command(OnScanAsync);
            UpdateDeliveryCommand = new Command(OnUpdateDelivery);
            UpdateSuccessCommand = new Command(OnUpdateSuccess);
            UpdatePendingCommand = new Command(OnUpdatePending);
            UpdateCarrierCommand = new Command(OnUpdateCarrier);
            UpdateFailedCommand = new Command(OnUpdateFailed);

            RefreshCommand = new Command(CmdRefresh);
            LoadPendingDocuments = new Command(async () => await ExecuteLoadItemsCommand());

        }


        #region "Command"


        public Command ScanCommand { get; }
        public Command LoadPendingDocuments { get; }
        public Command RefreshCommand { get; set; }
        public Command UpdateDeliveryCommand { get; }
        public Command UpdatePendingCommand { get; }
        public Command UpdateCarrierCommand { get; }


        public Command UpdateSuccessCommand { get; }
        public Command UpdateFailedCommand { get; }


        private async void CmdRefresh()
        {
            IsRefreshing = true;
            await Task.Delay(1000);
            IsRefreshing = false;
        }
        public void OnAppearing()
        {
            IsBusy = true;
            RefreshCommand.Execute(null);
            IsBusy = false;

        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {

                DeliveredDocuments.Clear();
                PendingDocuments.Clear();
                SuccessDocuments.Clear();
                FailedDocuments.Clear();
                SummaryDocuments.Clear();
                CarrierList.Clear();
                TransferDocuments.Clear();



                var Carriers = await CarrierDataStore.GetCarrierAsync(true);

                foreach (var carrierX in Carriers)
                {
                    CarrierList.Add(new Carrier()
                    {
                        // id = carrierX.id,
                        //CarrierCode = carrierX.CarrierCode,
                        CarrierName = carrierX.CarrierName

                    });
                }



                var items = await DataStore.GetItemsAsync(true);
                var sortedItems = items.OrderByDescending(c => c.OrderDate);




                foreach (var itemx in sortedItems)
                {
                    SummaryDocuments.Add(itemx);

                    if (itemx.OrderStatus == "Pending Confirmation")
                    {

                        DeliveredDocuments.Add(new Delivered()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus
                        });

                    }
                    else if (itemx.OrderStatus == "Transfer To Carrier")
                    {

                        TransferDocuments.Add(new Transfer()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus
                        });


                    }
                    else if (itemx.OrderStatus == "Pending")
                    {

                        PendingDocuments.Add(new Pending()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus
                        });


                    }
                    else if (itemx.OrderStatus == "Success")
                    {
                        SuccessDocuments.Add(new Success()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus
                        });
                    }
                    else if (itemx.OrderStatus == "Failed")
                    {
                        FailedDocuments.Add(new Failed()
                        {
                            Id = itemx.Id,
                            BookingId = itemx.BookingId,
                            OrderRefNo = itemx.OrderRefNo,
                            OrdeConsignee = itemx.OrdeConsignee,
                            OrderDate = itemx.OrderDate,
                            OrderDestination = itemx.OrderDestination,
                            OrderStatus = itemx.OrderStatus
                        });
                    }

                    // Load Carrier

                   

                   

                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnScanAsync(object obj)

        {

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            var result = await scanner.Scan();
            if (result != null)
            {
                MWABNo = result.Text;


            }


        }
        public async Task OnClickbackAsync()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
        public async void OnUpdateDelivery(object obj)
        {

            var nameOfPropertyId = "Id";
            var propertyInfoId = obj.GetType().GetProperty(nameOfPropertyId);
            var Id = propertyInfoId.GetValue(obj, null);


            var nameOfProperty = "BookingId";
            var propertyInfo = obj.GetType().GetProperty(nameOfProperty);
            var value = propertyInfo.GetValue(obj, null);

            var nameOfPropertyx = "OrderDestination";
            var propertyInfox = obj.GetType().GetProperty(nameOfPropertyx);
            var Xvalue = propertyInfox.GetValue(obj, null);

            var nameOfPropertyy = "OrderRefNo";
            var propertyInfoy = obj.GetType().GetProperty(nameOfPropertyy);
            var Yvalue = propertyInfoy.GetValue(obj, null);



            _RefNo = value.ToString();


            var client = new RestClient("https://18.139.49.140:8090/api/booking?$filter=bookingId eq " + "" + value + "");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddParameter("application/json", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                Booking.BookingStatus booker = new Booking.BookingStatus()
                {
                    bookingId = int.Parse(_RefNo),
                    refNo = Yvalue.ToString(),
                    destination = Xvalue.ToString(),
                    bookStatus = "Transfer To Carrier"


                };

                foreach (var x in result)
                {
                    x.bookingStatus.Clear();

                    x.bookingStatus.Add(booker);
                }

                // result.Add(booker);



                //var bookstatus = JsonConvert.SerializeObject(booker);
                // updateBook.bookingStatus.Add(bookstatus);

                string stringjson = JsonConvert.SerializeObject(result);
                string stringjsonX = stringjson.TrimStart('[').TrimEnd(']');



                var clientx = new RestClient("http://18.139.49.140:8089/api/booking/u");
                clientx.Timeout = -1;
                var requestx = new RestRequest(Method.PATCH);
                requestx.AddHeader("Content-Type", "application/json");
                requestx.AddParameter("application/json", JObject.Parse(stringjsonX), ParameterType.RequestBody);
                IRestResponse responsex = clientx.Execute(requestx);
                if (responsex.IsSuccessful == true)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Deliveries " + value + " with order reference # " + Yvalue.ToString() + " is now in Transfer To Carier status.", "Ok");
                    // MCA remove on Delivered
                    var itemX = DeliveredDocuments.First(c => c.Id.Equals(Id));
                    DeliveredDocuments.Remove(itemX);
                    // Push to Pending
                    TransferDocuments.Add(new Transfer()
                    {

                        Id = itemX.Id,
                        BookingId = itemX.BookingId,
                        OrderRefNo = itemX.OrderRefNo,
                        OrdeConsignee = itemX.OrdeConsignee,
                        OrderDate = itemX.OrderDate,
                        OrderDestination = itemX.OrderDestination,
                        OrderStatus = "Transfer To Carrier"

                    });

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", responsex.StatusDescription, "Ok");
                    return;
                }




            }



        }
        public async void OnUpdatePending(object obj)
        {
            var nameOfPropertyId = "Id";
            var propertyInfoId = obj.GetType().GetProperty(nameOfPropertyId);
            var Id = propertyInfoId.GetValue(obj, null);

            var nameOfProperty = "BookingId";
            var propertyInfo = obj.GetType().GetProperty(nameOfProperty);
            var value = propertyInfo.GetValue(obj, null);

            var nameOfPropertyx = "OrderDestination";
            var propertyInfox = obj.GetType().GetProperty(nameOfPropertyx);
            var Xvalue = propertyInfox.GetValue(obj, null);

            var nameOfPropertyy = "OrderRefNo";
            var propertyInfoy = obj.GetType().GetProperty(nameOfPropertyy);
            var Yvalue = propertyInfoy.GetValue(obj, null);



            _RefNo = value.ToString();


            var client = new RestClient("https://18.139.49.140:8090/api/booking?$filter=bookingId eq " + "" + value + "");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddParameter("application/json", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                Booking.BookingStatus booker = new Booking.BookingStatus()
                {
                    bookingId = int.Parse(_RefNo),
                    refNo = Yvalue.ToString(),
                    destination = Xvalue.ToString(),
                    bookStatus = "Success"


                };

                foreach (var x in result)
                {
                    x.bookingStatus.Clear();

                    x.bookingStatus.Add(booker);
                }

                // result.Add(booker);



                //var bookstatus = JsonConvert.SerializeObject(booker);
                // updateBook.bookingStatus.Add(bookstatus);

                string stringjson = JsonConvert.SerializeObject(result);
                string stringjsonX = stringjson.TrimStart('[').TrimEnd(']');



                var clientx = new RestClient("http://18.139.49.140:8089/api/booking/u");
                clientx.Timeout = -1;
                var requestx = new RestRequest(Method.PATCH);
                requestx.AddHeader("Content-Type", "application/json");
                requestx.AddParameter("application/json", JObject.Parse(stringjsonX), ParameterType.RequestBody);
                IRestResponse responsex = clientx.Execute(requestx);
                if (responsex.IsSuccessful == true)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Deliveries " + value + " with order reference # " + Yvalue.ToString() + " is now in Success status.", "Ok");

                    // MCA remove on Delivered
                    var itemX = PendingDocuments.First(c => c.Id.Equals(Id));
                    PendingDocuments.Remove(itemX);
                    // Push to Pending
                    SuccessDocuments.Add(new Success()
                    {

                        Id = itemX.Id,
                        BookingId = itemX.BookingId,
                        OrderRefNo = itemX.OrderRefNo,
                        OrdeConsignee = itemX.OrdeConsignee,
                        OrderDate = itemX.OrderDate,
                        OrderDestination = itemX.OrderDestination,
                        OrderStatus = "Success"

                    });

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", responsex.StatusDescription, "Ok");
                    return;
                }




            }



        }

        public async void OnUpdateCarrier(object obj)
        {

        }
        public async void OnUpdateSuccess(object obj)
        {
            var nameOfPropertyId = "Id";
            var propertyInfoId = obj.GetType().GetProperty(nameOfPropertyId);
            var Id = propertyInfoId.GetValue(obj, null);

            var nameOfProperty = "BookingId";
            var propertyInfo = obj.GetType().GetProperty(nameOfProperty);
            var value = propertyInfo.GetValue(obj, null);

            var nameOfPropertyx = "OrderDestination";
            var propertyInfox = obj.GetType().GetProperty(nameOfPropertyx);
            var Xvalue = propertyInfox.GetValue(obj, null);

            var nameOfPropertyy = "OrderRefNo";
            var propertyInfoy = obj.GetType().GetProperty(nameOfPropertyy);
            var Yvalue = propertyInfoy.GetValue(obj, null);



            _RefNo = value.ToString();


            var client = new RestClient("https://18.139.49.140:8090/api/booking?$filter=bookingId eq " + "" + value + "");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddParameter("application/json", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                Booking.BookingStatus booker = new Booking.BookingStatus()
                {
                    bookingId = int.Parse(_RefNo),
                    refNo = Yvalue.ToString(),
                    destination = Xvalue.ToString(),
                    bookStatus = "Failed"


                };

                foreach (var x in result)
                {
                    x.bookingStatus.Clear();

                    x.bookingStatus.Add(booker);
                }

                // result.Add(booker);



                //var bookstatus = JsonConvert.SerializeObject(booker);
                // updateBook.bookingStatus.Add(bookstatus);

                string stringjson = JsonConvert.SerializeObject(result);
                string stringjsonX = stringjson.TrimStart('[').TrimEnd(']');



                var clientx = new RestClient("http://18.139.49.140:8089/api/booking/u");
                clientx.Timeout = -1;
                var requestx = new RestRequest(Method.PATCH);
                requestx.AddHeader("Content-Type", "application/json");
                requestx.AddParameter("application/json", JObject.Parse(stringjsonX), ParameterType.RequestBody);
                IRestResponse responsex = clientx.Execute(requestx);
                if (responsex.IsSuccessful == true)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Deliveries " + value + " with order reference # " + Yvalue.ToString() + " is now in Failed status.", "Ok");

                    // MCA remove on Delivered
                    var itemX = SuccessDocuments.First(c => c.Id.Equals(Id));
                    SuccessDocuments.Remove(itemX);
                    // Push to Pending
                    FailedDocuments.Add(new Failed()
                    {

                        Id = itemX.Id,
                        BookingId = itemX.BookingId,
                        OrderRefNo = itemX.OrderRefNo,
                        OrdeConsignee = itemX.OrdeConsignee,
                        OrderDate = itemX.OrderDate,
                        OrderDestination = itemX.OrderDestination,
                        OrderStatus = "Failed"

                    });
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", responsex.StatusDescription, "Ok");
                    return;
                }




            }



        }
        public async void OnUpdateFailed(object obj)
        {

            var nameOfProperty = "BookingId";
            var propertyInfo = obj.GetType().GetProperty(nameOfProperty);
            var value = propertyInfo.GetValue(obj, null);

            var nameOfPropertyx = "OrderDestination";
            var propertyInfox = obj.GetType().GetProperty(nameOfPropertyx);
            var Xvalue = propertyInfox.GetValue(obj, null);

            var nameOfPropertyy = "OrderRefNo";
            var propertyInfoy = obj.GetType().GetProperty(nameOfPropertyy);
            var Yvalue = propertyInfoy.GetValue(obj, null);



            _RefNo = value.ToString();


            var client = new RestClient("https://18.139.49.140:8090/api/booking?$filter=bookingId eq " + "" + value + "");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddParameter("application/json", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful == true)
            {
                //var data = JsonConvert.DeserializeObject(response.Content)
                List<Booking> result = (List<Booking>)JsonConvert.DeserializeObject(response.Content, typeof(List<Booking>));


                Booking.BookingStatus booker = new Booking.BookingStatus()
                {
                    bookingId = int.Parse(_RefNo),
                    refNo = Yvalue.ToString(),
                    destination = Xvalue.ToString(),
                    bookStatus = "Success"


                };

                foreach (var x in result)
                {
                    x.bookingStatus.Clear();

                    x.bookingStatus.Add(booker);
                }

                // result.Add(booker);



                //var bookstatus = JsonConvert.SerializeObject(booker);
                // updateBook.bookingStatus.Add(bookstatus);

                string stringjson = JsonConvert.SerializeObject(result);
                string stringjsonX = stringjson.TrimStart('[').TrimEnd(']');



                var clientx = new RestClient("http://18.139.49.140:8089/api/booking/u");
                clientx.Timeout = -1;
                var requestx = new RestRequest(Method.PATCH);
                requestx.AddHeader("Content-Type", "application/json");
                requestx.AddParameter("application/json", JObject.Parse(stringjsonX), ParameterType.RequestBody);
                IRestResponse responsex = clientx.Execute(requestx);
                if (responsex.IsSuccessful == true)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Dliveriese " + RefNo + " already in " + StatusType + " Status.", "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", responsex.StatusDescription, "Ok");
                    return;
                }




            }



        }

        #endregion 

    }


}
