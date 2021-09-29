using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;


namespace Scanner.Droid
{
    [Activity(Label = "FileSaver")]
    public class FileSaverActivity : Activity
    {
        private byte[] PdfData = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.PdfData = FileSaver.PdfData;

            Intent intent = new Intent(Intent.ActionCreateDocument);
            intent.SetType("*/*");
            intent = Intent.CreateChooser(intent, "Save as pdf");
            base.StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 0 && resultCode == Result.Ok)
            {
                var uri = data.Data;
                var file_stream = ContentResolver.OpenOutputStream(uri, "rwt");
                file_stream.Write(PdfData, 0, PdfData.Length);
                file_stream.Flush();
                file_stream.Close();
                PdfData = null;
                Finish();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}