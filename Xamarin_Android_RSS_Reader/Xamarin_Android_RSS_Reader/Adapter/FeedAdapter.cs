using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Xamarin_Android_RSS_Reader.Interface;
using Xamarin_Android_RSS_Reader.Model;

namespace Xamarin_Android_RSS_Reader.Adapter
{
    class FeedViewHolder : RecyclerView.ViewHolder, View.IOnClickListener, View.IOnLongClickListener
    {

        public TextView txtTitle, txtContent, txtPubDate;
        public ItemClickListener ItemClickListener { get; set; }

        public FeedViewHolder(View itemView):base(itemView)
        {
            txtTitle = (TextView)itemView.FindViewById(Resource.Id.txtTitle);
            txtPubDate = (TextView)itemView.FindViewById(Resource.Id.txtPubDate);
            txtContent = (TextView)itemView.FindViewById(Resource.Id.txtContent);

            //set event
            itemView.SetOnClickListener(this);
            itemView.SetOnLongClickListener(this);


        }

        public void OnClick(View v)
        {
            ItemClickListener.OnClick(v, AdapterPosition, false);
        }

        public bool OnLongClick(View v)
        {
            ItemClickListener.OnClick(v, AdapterPosition, true);
            return true;
        }
    }

    public class FeedAdapter : RecyclerView.Adapter, ItemClickListener
    {
        private RssObject rssObject;
        private Context mContext;
        private LayoutInflater inflater;

        public FeedAdapter(RssObject rssObeject, Context mContext)
        {
            this.rssObject = rssObeject;
            this.mContext = mContext;
            inflater = LayoutInflater.From(mContext);
        }

        public override int ItemCount => rssObject.items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            FeedViewHolder hh = holder as FeedViewHolder;
            hh.txtTitle.Text = rssObject.items[position].title;
            hh.txtPubDate.Text = rssObject.items[position].pubDate;
            hh.txtContent.Text = rssObject.items[position].content;

            hh.ItemClickListener = this;


        }

        public void OnClick(View view, int position, bool isLongCLick)
        {
            if (!isLongCLick)
            {
                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(rssObject.items[position].link));
                mContext.StartActivity(intent);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = inflater.Inflate(Resource.Layout.Row, parent, false);
            return new FeedViewHolder(itemView);
        }
    }
}