﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Identity.Client;
using System.Collections.ObjectModel;

namespace CustomerApp
{
    public partial class MainPage : ContentPage
    {
        private AuthenticationResult authenticationResult;
        private ObservableCollection<ClaimViewModel> claimListViewSource = new ObservableCollection<ClaimViewModel>();
        private List<ClaimModel> claimListSource= new List<ClaimModel>();
        public MainPage(AuthenticationResult result)
        {
            InitializeComponent();
            this.authenticationResult = result;
            NavigationPage.SetHasNavigationBar(this, false);
            InitClaimList();
            listView.ItemTapped += async (sender, e) => {

                listView.SelectedItem = null;//clear android select color
                ClaimViewModel vmodel = (ClaimViewModel)e.Item;
                ClaimModel claim = claimListSource.Find(x => x.Id.Equals(vmodel.Id));
                if (claim != null) {
                   await Navigation.PushAsync(new ClaimDetailPage(claim));
                }
            };
            if(result != null){
                this.userNameLabel.Text = $"Hello {result.User.Name}";
            }

            InitGridView();

        }

        private void InitGridView()
        {
            if (mainPageGrid.RowDefinitions.Count == 0)
            {
                mainPageGrid.Padding =  new Thickness(Display.Convert(32), Display.Convert(30));

                listView.SeparatorVisibility = SeparatorVisibility.None;
                listView.ItemsSource = claimListViewSource;
                listView.ItemTemplate = new DataTemplate(typeof(ClaimCell));
                listView.RowHeight = Display.Convert(140);

                //android has status bar
                Display.SetGridRowHeight(mainPageGrid, 10, 0.5);
                int[] rowsHeight = new int[] { 90, 234, 90, 48, 392, 30, 380, 30};
                Display.SetGridRowsHeight(mainPageGrid, rowsHeight);
                Display.SetGridRowsStarHeight(mainPageGrid, new int[] { 1 });


                Display.SetGridRowsHeight(tab1, new int[] { 40, 54});
                Display.SetGridRowsStarHeight(tab1, new int[] { 1 });
                Display.SetGridRowsHeight(tab1, new int[] { 16 });

                Display.SetGridRowsHeight(tab2, new int[] { 20, 94 });
                Display.SetGridRowsStarHeight(tab2, new int[] { 1 });
                Display.SetGridRowsHeight(tab2, new int[] { 30});
                Display.SetGridRowsStarHeight(tab2, new int[] { 1 });
                Display.SetGridRowsHeight(tab2, new int[] { 50 });
            }

        }
        private void InitClaimList() {

            claimListSource.Add(new ClaimModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Garage Flooding",
                Status = "Processing",
                ClaimDateTime = DateTime.Now,
                ImageUrl = "demo1.png",
                ClaimDescription = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Fusce dapibus, tellus ac cursur commondo, tortor manuris condimentum nibh, ut fermentum massa justo sit amet risus.",
            });
            claimListSource.Add(new ClaimModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Thunderstorm",
                Status = "Approved",
                ClaimDateTime = DateTime.Now,
                ImageUrl = "demo2.png",
                ClaimDescription = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Fusce dapibus, tellus ac cursur commondo, tortor manuris condimentum nibh, ut fermentum massa justo sit amet risus.",
            });
            UpdateClaimViewList();

        }
        private void UpdateClaimViewList()
        {
            //claimListSource
            claimListViewSource.Clear();
            for (int index = 0; index < claimListSource.Count; index++)
            {
                ClaimViewModel viewModel = new ClaimViewModel()
                {
                    Id = claimListSource[index].Id,
                    Index = (index + 1).ToString(),
                    Name = claimListSource[index].Name,
                    Status = claimListSource[index].Status
                };
                claimListViewSource.Add(viewModel);
            }


        }
        public void AddNewClaim(ClaimModel cl) {
            this.claimListSource.Insert(0, cl);
            UpdateClaimViewList();
        }
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.PCApplication.UserTokenCache.Clear(App.ClientID);
            await Navigation.PopAsync();
        }
        private async void OnNewClaimButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewClaimPage(this.authenticationResult, this));
        }
    }
}
