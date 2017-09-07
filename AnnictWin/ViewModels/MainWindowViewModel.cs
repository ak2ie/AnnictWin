using AnnictWin.Models;
using Prism.Mvvm;

namespace AnnictWin.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Autofac Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
            AnimeList = "リスト取得前";

            Annict annict = new Annict();
            annict.test();
        }

        private string _animeList;
        public string AnimeList
        {
            get { return _animeList; }
            set { SetProperty(ref _animeList, value); }
        }
    }
}
