
// Code in VMLocator is borrowed from John Papa
// For reference, see: http://johnpapa.net/silverlight/simple-viewmodel-locator-for-mvvm-the-patients-have-left-the-asylum/


namespace Pippin.VMLocator
{
    public class ViewModelLocator
    {
        public ViewModelIndexer Find { get; private set; }
        public ViewModelIndexer FindShared { get; private set; }

        public ViewModelLocator()
        {
            Find = new ViewModelIndexer(){IsShared = false};
            FindShared = new ViewModelIndexer(){IsShared = true};
        }
    }

}
