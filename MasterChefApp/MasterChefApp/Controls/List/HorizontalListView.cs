using MasterChefApp.Controls.List;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MasterChefApp.Controls
{
    public class HorizontalListView: ContentView
    {
        public static readonly BindableProperty ItemSourcesProperty =
        BindableProperty.Create(
            nameof(ItemSources),
            typeof(IEnumerable<object>),
            typeof(HorizontalListView),
            null, propertyChanged: OnpropertyChanged);

        private static void OnpropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var collection = newValue as IEnumerable<object>;
            if (collection == null) return;
            var Parent = bindable as HorizontalListView;
            foreach (var item in collection)
            {
                Parent.AddLast(item);
            }
            Parent.IsLoading = false;
            
        }

        public int MainHeight { get; set; }

        public static readonly BindableProperty BorderItemColorProperty =
        BindableProperty.Create(
            nameof(BorderItemColor),
            typeof(Color),
            typeof(BorderEntry),
            Color.Gray);

        // Gets or sets BorderItemColor value
        public Color BorderItemColor
        {
            get { return (Color)GetValue(BorderItemColorProperty); }
            set { SetValue(BorderItemColorProperty, value); }
        }
        //BorcerItemColor
        public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(BorderEntry),
            true);

        // Gets or sets IsLoading value
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); waitingGrid.IsVisible = IsLoading; }
        }

        // Gets or sets ItemSources value
        public IEnumerable<object> ItemSources
        {
            get { return (IEnumerable<object>)GetValue(ItemSourcesProperty); }
            set { SetValue(ItemSourcesProperty, value); }
        }


        private ScrollView scroll;
        public StackLayout MainContent;
        Grid waitingGrid;
        public HorizontalListView()
        {
            Grid grid = new Grid();
            
            scroll = new ScrollView() { Orientation = ScrollOrientation.Horizontal, HeightRequest = 150 };
            MainContent = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 150 };
            this.HeightRequest = 150;
            scroll.Content = MainContent;

            grid.Children.Add(scroll);

            waitingGrid = new Grid();
            waitingGrid.Children.Add(new ActivityIndicator() { IsRunning = true, HeightRequest=50 ,WidthRequest = 50, Color = Color.FromHex("#6a27e6"),
                VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center});
            grid.Children.Add(waitingGrid);
            Content = grid;
        }

        public void AddFirst(object item)
        {
            Device.BeginInvokeOnMainThread(() => {
                MainContent.Children.Insert(0, new HorizontalListItem() { BindingContext = item, BorderItemColor = BorderItemColor });
            });
        }
        public void AddLast(object item)
        {
            Device.BeginInvokeOnMainThread(() => {
                MainContent.Children.Add(new HorizontalListItem() { BindingContext = item, BorderItemColor = BorderItemColor });
            });
        }

        public void Remove(object Item)
        {
            Device.BeginInvokeOnMainThread(() => {
                var removeItem = MainContent.Children.FirstOrDefault(x => x.BindingContext == Item);
                int index = ItemSources.ToList().IndexOf(removeItem);
                MainContent.Children.RemoveAt(index);
            });
        }

        public void UpdateItem(object oldItem, object newItem)
        {
            Device.BeginInvokeOnMainThread(() => {
                var removeItem = MainContent.Children.FirstOrDefault(x => x.BindingContext == oldItem);
                int index = ItemSources.ToList().IndexOf(removeItem);
                MainContent.Children.RemoveAt(index);
                MainContent.Children.Insert(index, new HorizontalListItem() { BindingContext =  newItem, BorderItemColor = BorderItemColor });
            });
        }
    }
}
