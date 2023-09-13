using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Mvvm.Messaging;
using StepCounter;
using StepCounter.Global;
using static Android.Provider.CalendarContract;

namespace MauiWidgets.Platforms.Android
{
    [BroadcastReceiver(Exported = false, Enabled = true)]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/example_appwidget_info")]
    public class SampleWidget : AppWidgetProvider
    {
        private const string ActionIdentifier = "test";
        RemoteViews vwWidget;
        int widgetId;
        AppWidgetManager appWidgetManager;

        public override async void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            this.appWidgetManager = appWidgetManager;
            foreach (var widgetId in appWidgetIds)
            {
                var intent = new Intent(context, typeof(MainActivity));
                intent.SetAction(ActionIdentifier);
                intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
                var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable);

                this.widgetId = widgetId;
                vwWidget = new RemoteViews(packageName: context.PackageName, layoutId: StepCounter.Resource.Layout.WidgetLayout);
                vwWidget.SetOnClickPendingIntent(StepCounter.Resource.Id.widgetLayout, pendingIntent);

                // Tell the AppWidgetManager to perform an update on the current app widget.
                appWidgetManager.UpdateAppWidget(StepCounter.Resource.Layout.WidgetLayout, vwWidget);
            }

            if (App.IsDatabaseInitialized == false)
                return;

            if (vwWidget == null)
                return;

            var currentS = await App.Database.GetCurrent();
            vwWidget.SetTextViewText(StepCounter.Resource.Id.textView2, currentS.Steps.ToString());
            appWidgetManager.UpdateAppWidget(widgetId, vwWidget);

        }

        public override async void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if (App.IsDatabaseInitialized == false)
                return;

            var vwWidget = new RemoteViews(packageName: context.PackageName, layoutId: StepCounter.Resource.Layout.WidgetLayout);
            if (vwWidget == null)
                return;

            var appWidgetManager = AppWidgetManager.GetInstance(MainActivity.Instance);

            var currentS = await App.Database.GetCurrent().ConfigureAwait(false);   
            vwWidget.SetTextViewText(StepCounter.Resource.Id.textView2, currentS.Steps.ToString());
            appWidgetManager.UpdateAppWidget(widgetId, vwWidget);
            
        }
    }
}