using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Views;
using Android.Widget;
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

        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            foreach (var widgetId in appWidgetIds)
            {
                var intent = new Intent(context, typeof(MainActivity));
                intent.SetAction(ActionIdentifier);
                var pendingIntent = PendingIntent.GetActivity(context, 1, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable);

                var views = new RemoteViews(packageName: context.PackageName, layoutId: StepCounter.Resource.Layout.WidgetLayout);
                views.SetOnClickPendingIntent(StepCounter.Resource.Id.actions, pendingIntent);

                // Tell the AppWidgetManager to perform an update on the current app widget.
                appWidgetManager.UpdateAppWidget(widgetId, views);
            }
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;
            if (action == ActionIdentifier)
            {
                MessagingCenter.Send<WidgetActionMessage>(new WidgetActionMessage(), string.Empty);
            }
            base.OnReceive(context, intent);
        }
    }
}