﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using JiHuangBaiKeForUWP.Model;
using JiHuangBaiKeForUWP.UserControls;

namespace JiHuangBaiKeForUWP.View.Dialog
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ScienceDialog : Page
    {
        private string _unlockCharcter;
        public ScienceDialog(Science c)
        {
            this.InitializeComponent();

            ScienceImage.Source = new BitmapImage(new Uri(c.Picture));
            ScienceName.Text = c.Name;
            ScienceEnName.Text = c.EnName;
            Need1PicButton.Source = StringProcess.GetGameResourcePath(c.Need1);
            Need1PicButton.Text = $"×{c.Need1Value}";
            if (c.Need2 != null)
            {
                Need2PicButton.Source = StringProcess.GetGameResourcePath(c.Need2);
                Need2PicButton.Text = $"×{c.Need2Value}";
                Need2PicButton.Visibility = Visibility.Visible;
            }
            if (c.Need3 != null)
            {
                Need3PicButton.Source = StringProcess.GetGameResourcePath(c.Need3);
                Need3PicButton.Text = $"×{c.Need3Value}";
                Need3PicButton.Visibility = Visibility.Visible;
            }
            if (c.Unlock == null && c.UnlockCharcter == null && c.UnlockBlueprint == null)
            {
                ScienceUnlockStackPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (c.Unlock != null)
                {
                    UnlockPicButton.Visibility = Visibility.Visible;
                    UnlockPicButton.Source = StringProcess.GetGameResourcePath(c.Unlock);
                }
                if (c.UnlockCharcter != null)
                {
                    UnlockCharcterButton.Visibility = Visibility.Visible;
                    UnlockCharcterImage.Source = new BitmapImage(new Uri(StringProcess.GetGameResourcePath(c.UnlockCharcter)));
                    _unlockCharcter = StringProcess.GetGameResourcePath(c.UnlockCharcter);
                }
                if (c.UnlockBlueprint != null)
                {
                    UnlockBlueprintPicButton.Visibility = Visibility.Visible;
                    UnlockBlueprintPicButton.Source = StringProcess.GetGameResourcePath(c.UnlockBlueprint);
                }
            }
            ScienceIntroduction.Text = c.Introduction;
            ConsolePre.Text = $"c_give(\"{c.Console}\",";
        }

        private void ConsoleNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            StringProcess.ConsoleNumTextCheck(textbox);
        }

        private void Copy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(ConsolePre.Text + ConsoleNum.Text + ")");
            Clipboard.SetContent(dataPackage);
        }

        private async void Science_Jump_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var picturePath = ((PicButton)sender).Source;
            var rootFrame = Global.RootFrame;
            var shortName = StringProcess.GetFileName(picturePath);
            var frameTitle = Global.FrameTitle;
            await Global.SetAutoSuggestBoxItem();
            foreach (var suggestBoxItem in Global.AutoSuggestBoxItemSource)
            {
                if (picturePath != suggestBoxItem.Picture) continue;
                var picHead = shortName.Substring(0, 1);
                string[] extraData = { suggestBoxItem.SourcePath, suggestBoxItem.Picture }; ;
                switch (picHead)
                {
                    case "F":
                        frameTitle.Text = "食物";
                        Global.PageJump(1);
                        rootFrame.Navigate(typeof(FoodPage), extraData);
                        break;
                    case "S":
                        rootFrame.Navigate(typeof(SciencePage), extraData);
                        break;
                    case "G":
                        frameTitle.Text = "物品";
                        Global.PageJump(6);
                        rootFrame.Navigate(typeof(GoodPage), extraData);
                        break;
                }
            }
        }

        private async void Science_CharacterJump_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var picturePath = _unlockCharcter;
            var rootFrame = Global.RootFrame;
            var frameTitle = Global.FrameTitle;
            await Global.SetAutoSuggestBoxItem();
            foreach (var suggestBoxItem in Global.AutoSuggestBoxItemSource)
            {
                if (picturePath != suggestBoxItem.Picture) continue;
                string[] extraData = { suggestBoxItem.SourcePath, suggestBoxItem.Picture }; ;
                frameTitle.Text = "人物";
                Global.PageJump(0);
                rootFrame.Navigate(typeof(CharacterPage), extraData);
            }
        }
    }
}
