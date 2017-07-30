using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Xamarin_Android_RSS_Reader.Model;
using System;
using Xamarin_Android_RSS_Reader.Common;
using Java.Lang;
using Newtonsoft.Json;
using Xamarin_Android_RSS_Reader.Adapter;
using Android.Views;

namespace Xamarin_Android_RSS_Reader
{
    [Activity(Label = "Xamarin_Android_RSS_Reader", MainLauncher = true, Icon = "@drawable/icon",Theme ="@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {

        Android.Support.V7.Widget.Toolbar toolbar;
        RecyclerView recyclreView;
        RssObject rssObject;

        private const string RSS_Link = "http://rss.nytimes.com/services/xml/rss/nyt/Science.xml";
        private const string RSS_to_json = "https://api.rss2json.com/v1/api.json?rss_url=";

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main_Menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_refresh) ;
                LoadData();
            return true;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "News";
            SetSupportActionBar(toolbar);

            recyclreView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            recyclreView.SetLayoutManager(linearLayoutManager);

            LoadData();

        }

        private void LoadData()
        {
            StringBuilder strBuilder = new StringBuilder(RSS_to_json);
            strBuilder.Append(RSS_Link);

            new LoadDataAsync(this).Execute(strBuilder.ToString());
        }

        class LoadDataAsync : AsyncTask<string, string, string>
        {
            MainActivity mainActivity;
            ProgressDialog mDialog;

            public LoadDataAsync (MainActivity mainActivity)
            {
                this.mainActivity = mainActivity;
            }

            protected override void OnPreExecute()
            {
                mDialog = new ProgressDialog(mainActivity);
                mDialog.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
                mDialog.SetMessage("Please wait...");
                mDialog.Show();
            }

            protected override string RunInBackground(params string[] @params)
            {
                string result = new HTTPDataHandler().GetHTTPData(@params[0]);
                return result;
            }

            protected override void OnPostExecute(string result)
            {
                RssObject data = JsonConvert.DeserializeObject<RssObject>(result);
                mDialog.Dismiss();
                FeedAdapter adapter = new FeedAdapter(data, mainActivity);
                mainActivity.recyclreView.SetAdapter(adapter);
                adapter.NotifyDataSetChanged();
            }
        }
    }
}

