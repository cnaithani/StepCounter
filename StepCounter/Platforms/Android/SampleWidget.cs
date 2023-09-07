using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Mvvm.Messaging;
using StepCounter;
using StepCounter.Global;

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

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
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


            WeakReferenceMessenger.Default.Register<StepStepUpdateMsg>(this, async (m, e) =>
            {
                if (App.IsDatabaseInitialized == false)
                    return;

                if (vwWidget == null)
                    return;

                var currentS = await App.Database.GetCurrent();
                vwWidget.SetTextViewText(StepCounter.Resource.Id.textView2, currentS.Steps.ToString());
                appWidgetManager.UpdateAppWidget(widgetId, vwWidget);
            });
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;
            if (action == ActionIdentifier)
            {
                //MessagingCenter.Send<WidgetActionMessage>(new WidgetActionMessage(), string.Empty);
            }
            base.OnReceive(context, intent);
        }
    }
}