using DormitoryGUI.Model;
using DormitoryGUI.View;
using DormitoryGUI.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace DormitoryGUI
{
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page
    {
        private StudentList listviewCollection;
        private StudentList resultListCollection;
        private StudentList mpoint_listviewCollection;
        private JArray mpoint_studentList;


        private JArray studentList;

        private readonly MainWindow mainWindow;

        private string mpoint_filter = "전체";

        private string filter = "전체";

        public MainPage(MainWindow mainWindow)
        {
            InitializeComponent();

            listviewCollection = Resources["StudentListKey"] as ViewModel.StudentList;
            resultListCollection = Resources["ResultListKey"] as ViewModel.StudentList;
            mpoint_listviewCollection = Resources["mpoint_StudentListKey"] as ViewModel.StudentList;


            Update();

            this.mainWindow = mainWindow;
            RuleList_bad.Visibility = Visibility.Hidden;
        }

        private void All_Grade_Click(object sender, RoutedEventArgs e)
        {
            if (SearchList.Visibility == Visibility.Visible)
            {
                var bc = new BrushConverter();
                Console.WriteLine("a");

                All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
                First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
                Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
                Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
                All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
                First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
                Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
                Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");

                filter = "전체";
                Update();
            }
            else
            {
                mpoint_Update();
            }
        }

        private void First_Grade_Click(object sender, RoutedEventArgs e)
        {
            if (SearchList.Visibility == Visibility.Visible)
            {
                filter = "1학년";
                Update();
            }
            else
            {
                FilteringStudent(1);
            }

            var bc = new BrushConverter();
            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
        }

        private void Second_Grade_Click(object sender, RoutedEventArgs e)
        {
            if (SearchList.Visibility == Visibility.Visible)
            {
                filter = "2학년";
                Update();
            }
            else
            {
                FilteringStudent(2);
            }

            var bc = new BrushConverter();

            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
        }

        private void Third_Grade_Click(object sender, RoutedEventArgs e)
        {
            if (SearchList.Visibility == Visibility.Visible)
            {
                filter = "3학년";
                Update();
            }
            else
            {
                FilteringStudent(3);
            }

            var bc = new BrushConverter();

            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
        }
        private void Good_Point_Button_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            Good_Point_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Bad_Point_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Good_Point_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Bad_Point_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            RuleList_good.Visibility = Visibility.Visible;
            RuleList_bad.Visibility = Visibility.Hidden;
        }

        private void Bad_Point_Button_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            Good_Point_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Bad_Point_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Good_Point_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Bad_Point_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            RuleList_good.Visibility = Visibility.Hidden;
            RuleList_bad.Visibility = Visibility.Visible;
        }
        public void Update()
        {
            listviewCollection.Clear();
            var responseDict = Info.GenerateRequest("GET", Info.Server.MANAGING_STUDENT, Info.mainPage.AccessToken, "");

            if ((HttpStatusCode)responseDict["status"] != HttpStatusCode.OK)
            {
                Info.RefreshToken();
                responseDict = Info.GenerateRequest("GET", Info.Server.MANAGING_STUDENT, Info.mainPage.AccessToken, "");
                //MessageBox.Show("오류");
            }

            JObject responseJSON = JObject.Parse(responseDict["body"].ToString());
            studentList = JArray.Parse(responseJSON["studentList"].ToString());
            foreach (JObject student in studentList)
            {
                if ((filter != "전체") && (student["number"].ToString()[0] != filter[0]))
                {
                    continue;
                }

                StudentListViewModel item = new ViewModel.StudentListViewModel(
                    id: student["id"].ToString(),
                    classNumber: student["number"].ToString(),
                    name: student["name"].ToString(),
                    goodPoint: student["goodPoint"].Type == JTokenType.Null
                        ? 0
                        : int.Parse(student["goodPoint"].ToString()),
                    badPoint: student["badPoint"].Type == JTokenType.Null
                        ? 0
                        : int.Parse(student["badPoint"].ToString()),
                    penaltyTrainingStaus: bool.Parse(student["penaltyTrainingStatus"].ToString()),
                    penaltyLevel: bool.Parse(student["penaltyTrainingStatus"].ToString()) == true
                        ? Info.ParseStatus((int)student["penaltyLevel"])
                        : " ",
                    isSelected: false
                );

                //JArray logs = student["pointHistories"] as JArray;

                //foreach (JObject log in logs)
                //{
                ///////item.PunishLogs.Add(new PunishLogListViewModel(
                //score: (int) log["point"],
                //reason: log["reason"].ToString(),
                ////time: log["time"].ToString(),
                //                        time: DateTime.Parse(log["time"].ToString()).ToString("yyyy-MM-dd"),
                // pointType:
                //bool.Parse(log["pointType"].ToString())
                // ));
                //  }
                listviewCollection.Add(item);
            }

            Point_List();
        }

        private List<ListViewItem> Itemss_good = new List<ListViewItem>();
        private List<ListViewItem> Itemss_bad = new List<ListViewItem>();


        public void Point_List()
        {
            var responseDict = Info.GenerateRequest("GET", Info.Server.MANAGING_RULE, Info.mainPage.AccessToken, "");


            if ((HttpStatusCode)responseDict["status"] != HttpStatusCode.OK)
            {
                Info.RefreshToken();
                responseDict = Info.GenerateRequest("GET", Info.Server.MANAGING_RULE, Info.mainPage.AccessToken, "");
                //MessageBox.Show("오류");
            }

            JObject responseJSON = JObject.Parse(responseDict["body"].ToString());
            var rules = JArray.Parse(responseJSON["ruleList"].ToString());
            int item_cnt_good = 0, item_cnt_bad = 0;

            foreach (JObject rule in rules)
            {
                int point = rule["point"].Type == JTokenType.Null ? 0 : (int)rule["point"];

                //                if ((pointType == 0 && minPoint < 0) || (pointType == 1 && minPoint > 0))
                ListViewItem OneItem = new ListViewItem();

                if ((bool)rule["pointType"])
                {
                    OneItem.Content = new PointItem_good() { reason = (string)rule["reason"], point = (int)rule["point"], id = (int)rule["id"] };
                    Itemss_good.Add(OneItem);
                    RuleList_good.ItemsSource = Itemss_good;
                    Itemss_good[item_cnt_good++].FontSize = 18;
                }
                else
                {
                    OneItem.Content = new PointItem_bad() { reason = (string)rule["reason"], point = (int)rule["point"], id = (int)rule["id"] };

                    Itemss_bad.Add(OneItem);
                    RuleList_bad.ItemsSource = Itemss_bad;
                    Itemss_bad[item_cnt_bad++].FontSize = 18;
                }
                Console.WriteLine(rule["reason"]);
                //listviewCollection.Add(new PunishmentListViewModel(id: (string)rule["id"],name: (string)rule["reason"], minPoint: (int)rule["point"],maxPoint: (int)rule["point"]));
            }
            RuleList_good.Items.Refresh();
            RuleList_bad.Items.Refresh();

        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var target = (StudentListViewModel)GetAncestorOfType<ListViewItem>(sender as Button).DataContext;
            resultListCollection.Remove(target);

            ResultList.ItemsSource = resultListCollection;
            ResultList.Items.Refresh();
        }

        private void SearchList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listView = sender as ListView;
            var gridView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 18;

            double[] columnRatio =
            {
                0.2,
                0.2,
                0.2,
                0.2,
                0.2,
            };

            foreach (var element in gridView.Columns)
                element.Width = workingWidth * columnRatio[gridView.Columns.IndexOf(element)];
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string command = SearchCommand.Text;
            listviewCollection.Clear();

            foreach (JObject student in studentList)
            {
                if (student["number"].ToString().Contains(command) ||
                    student["name"].ToString().Contains(command) ||
                    student["goodPoint"].ToString().Contains(command) ||
                    student["badPoint"].ToString().Contains(command))
                {
                    listviewCollection.Add(new ViewModel.StudentListViewModel(
                        id: student["id"].ToString(),
                        classNumber: student["number"].ToString(),
                        name: student["name"].ToString(),
                        goodPoint: student["goodPoint"].Type == JTokenType.Null ? 0 : (int)student["goodPoint"],
                        badPoint: student["badPoint"].Type == JTokenType.Null ? 0 : (int)student["badPoint"],
                        penaltyTrainingStaus: bool.Parse(student["penaltyTrainingStatus"].ToString()),
                        penaltyLevel: bool.Parse(student["penaltyTrainingStatus"].ToString()) == true
                            ? Info.ParseStatus((int)student["penaltyLevel"])
                            : " ",
                        isSelected: false));
                }
            }
        }

        private void SearchCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchButton_Click(sender, null);
        }

        private void ResultList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listView = sender as ListView;
            var gridView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 18;

            double[] columnRatio =
            {
                0.33,
                0.33,
                0.33
            };

            foreach (var element in gridView.Columns)
                element.Width = workingWidth * columnRatio[gridView.Columns.IndexOf(element)];
        }
        int all_button_flag = 0;
        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            if (all_button_flag == 0)
            {
                foreach (var item in listviewCollection)
                {
                    SearchList.SelectedItems.Add(item);
                }
                all_button_flag = 1;
            }
            else
            {
                foreach (var item in listviewCollection)
                {
                    SearchList.SelectedItems.Remove(item);
                }
                all_button_flag = 0;

            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (StudentListViewModel element in SearchList.SelectedItems)
            {
                resultListCollection.Add(
                    new StudentListViewModel(
                        id: element.ID,
                        name: element.Name,
                        classNumber: element.ClassNumber,
                        goodPoint: element.GoodPoint,
                        badPoint: element.BadPoint,
                        penaltyTrainingStaus: element.PenaltyTrainingStatus,
                        penaltyLevel: element.PenaltyLevel,
                        isSelected: false
                    )
                );
            }

            ResultList.ItemsSource = Deduplication(resultListCollection as IEnumerable<StudentListViewModel>);
            ResultList.Items.Refresh();

            SearchList.Items.Refresh();
        }

        private T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);

            if (parent != null && !(parent is T))
                return GetAncestorOfType<T>((FrameworkElement)parent);

            return (T)parent;
        }

        private IEnumerable<StudentListViewModel> Deduplication(IEnumerable<StudentListViewModel> source)
        {
            return source.GroupBy(x => x.ClassNumber)
                .Select(y => y.First());
        }

        private void ResultList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DeleteSelectedItem();
        }

        private void DeleteSelectedItem()
        {
            // ResultList의 Selected 된 아이템 제거

            foreach (StudentListViewModel item in ResultList.SelectedItems)
            {
                resultListCollection.Remove(item);
            }

            // 이후 이를 ResultList의 ItemSource에 대입하고 Refresh

            ResultList.ItemsSource = resultListCollection;
            ResultList.Items.Refresh();
        }

        private bool? ShowModal(Window modal)
        {
            OpacityBox.Visibility = Visibility.Visible;

            var result = modal.ShowDialog();

            OpacityBox.Visibility = Visibility.Hidden;

            return result;
        }

        private void GradeCombobox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            filter = (sender as ComboBoxItem).Content.ToString();

            if (listviewCollection != null)
            {
                Update();
            }
        }

        private void SelectAllCheck_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in listviewCollection)
            {
                SearchList.SelectedItems.Add(item);
            }
        }

        private void SelectAllCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in listviewCollection)
            {
                SearchList.SelectedItems.Remove(item);
            }
        }

        private void DownloadExcel_Click(object sender, RoutedEventArgs e)
        {
            var responseDict = Info.GenerateRequest("GET", Info.Server.MANAGING_STUDENT, Info.mainPage.AccessToken, "");

            if ((HttpStatusCode)responseDict["status"] != HttpStatusCode.OK)
            {
                Info.RefreshToken();
                responseDict = Info.GenerateRequest("GET", Info.Server.MANAGING_STUDENT, Info.mainPage.AccessToken, "");
                //MessageBox.Show("오류");
            }

            JObject responseJSON = JObject.Parse(responseDict["body"].ToString());
            studentList = JArray.Parse(responseJSON["studentList"].ToString());
            string[] id_list = new string[240];
            string[] number_list = new string[240];
            Console.WriteLine(number_list.Length);
            Dictionary<string, JArray> dict = new Dictionary<string, JArray>();
            int count = 0;
            foreach (JObject student in studentList)
            {
                if ((filter != "전체") && (student["number"].ToString()[0] != filter[0]))
                {
                    continue;
                }

                StudentListViewModel item = new ViewModel.StudentListViewModel(
                    id: student["id"].ToString(),
                    classNumber: student["number"].ToString(),
                    name: student["name"].ToString(),
                    goodPoint: student["goodPoint"].Type == JTokenType.Null
                        ? 0
                        : int.Parse(student["goodPoint"].ToString()),
                    badPoint: student["badPoint"].Type == JTokenType.Null
                        ? 0
                        : int.Parse(student["badPoint"].ToString()),
                    penaltyTrainingStaus: bool.Parse(student["penaltyTrainingStatus"].ToString()),
                    penaltyLevel: bool.Parse(student["penaltyTrainingStatus"].ToString()) == true
                        ? Info.ParseStatus((int)student["penaltyLevel"])
                        : " ",
                    isSelected: false
                );
                id_list[count] = item.ID;
                number_list[count] = item.ClassNumber;
                count += 1;
            }

            for (int i = 0; id_list[i] != null; i++)
            {
                Console.WriteLine(id_list[i]);
                Console.WriteLine(number_list[i]);
                string id = id_list[i];
                var responseDict_ = Info.GenerateRequest("GET", $"{Info.Server.MANAGING_POINT}/{id}",
                    Info.mainPage.AccessToken, "");
                if ((HttpStatusCode)responseDict_["status"] != HttpStatusCode.OK)
                {
                    Info.RefreshToken();
                    responseDict_ = Info.GenerateRequest("GET", $"{Info.Server.MANAGING_POINT}/{id}",
                    Info.mainPage.AccessToken, "");
                    //MessageBox.Show("오류");
                }
                JObject responseJSON_ = JObject.Parse(responseDict_["body"].ToString());
                JArray logs = JArray.Parse(responseJSON_["point_history"].ToString());

                for (int j = logs.Count - 1; j >= 0; j--)
                {
                    JObject log = (JObject)logs[j];
                }
                dict.Add(number_list[i], logs);
            }

            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            if (!(bool)saveFileDialog.ShowDialog())
            {
                return;
            }

            var dataSet = new DataSet();
            Console.WriteLine(listviewCollection);
            for (int i = 1; i <= 3; i++)
            {
                var items = listviewCollection.Where(s => s.ClassNumber.StartsWith(i.ToString()));
                Console.WriteLine(items.Count());
                var dataTable = new DataTable(string.Format("{0}학년", i));
                dataTable.Columns.Add("학번");
                dataTable.Columns.Add("이름");
                dataTable.Columns.Add("상점");
                dataTable.Columns.Add("벌점");
                dataTable.Columns.Add("상점 내역");
                dataTable.Columns.Add("벌점 내역");
                dataTable.Columns.Add("교육 단계");
                foreach (StudentListViewModel item in items)
                {
                    StringBuilder goodLogsBuilder = new StringBuilder();
                    StringBuilder badLogsBuilder = new StringBuilder();

                    for (int a = 0; a < dict[item.ClassNumber].Count; a++)
                    {
                        if ((dict[item.ClassNumber][a]["pointType"]).ToString() == "True")
                        {
                            goodLogsBuilder.AppendFormat("[{0}] {1} ({2}점) \n", dict[item.ClassNumber][a]["date"], dict[item.ClassNumber][a]["reason"], dict[item.ClassNumber][a]["point"]);
                        }
                        else
                        {
                            badLogsBuilder.AppendFormat("[{0}] {1} ({2}점) \n", dict[item.ClassNumber][a]["date"], dict[item.ClassNumber][a]["reason"], dict[item.ClassNumber][a]["point"]);
                        }
                    }

                    int goodLogsCount = goodLogsBuilder.ToString().Length;
                    string goodLogsString = (goodLogsCount > 0)
                        ? goodLogsBuilder.ToString().Substring(0, goodLogsCount - 1)
                        : string.Empty;
                    int badLogsCount = badLogsBuilder.ToString().Length;
                    string badLogsString = (badLogsCount > 0)
                        ? badLogsBuilder.ToString().Substring(0, badLogsCount - 1)
                        : string.Empty;
                    dataTable.Rows.Add(
                        new object[]
                        {
                            item.ClassNumber,
                            item.Name,
                            item.GoodPoint,
                            item.BadPoint,
                            goodLogsString,
                            badLogsString,
                            item.PenaltyTrainingStatus == false && item.PenaltyLevel != " "
                                ? item.PenaltyLevel + "(상쇄완료)"
                                : item.PenaltyLevel
                        }
                    );
                }
                dataSet.Tables.Add(dataTable);

            }
            if (ExcelProcessing.SaveExcelDB(saveFileDialog.FileName, dataSet))
            {
                MessageBox.Show("엑셀 데이터 저장 성공");
            }

            else
            {
                MessageBox.Show("엑셀 데이터 저장 실패");
            }
        }

        private void WatchLogButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchList.SelectedItems.Count != 1)
            {
                MessageBox.Show("잘못된 접근입니다");
                return;
            }

            var target = (StudentListViewModel)SearchList.SelectedItems[0];

            var punishmentLogDialog =
                new PunishmentLogDialog(
                    id: target.ID,
                    name: target.Name,
                    classNumber: target.ClassNumber,
                    goodPoint: target.GoodPoint,
                    badPoint: target.BadPoint,
                    currentStep: target.PenaltyLevel
                );

            ShowModal(punishmentLogDialog);

            resultListCollection.Clear();

            ResultList.ItemsSource = resultListCollection;
            ResultList.Items.Refresh();

            Update();
        }

        private void SearchCommand_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Itemss.Clear();
            PointList.Items.Refresh();
            if (SearchList.SelectedItems.Count == 1)
            {
                var target = (StudentListViewModel)SearchList.SelectedItems[0];
                SetLogData(target.ID);
            }
        }

        private List<ListViewItem> Itemss = new List<ListViewItem>();

        private void SetLogData(string id)
        {
            var responseDict = Info.GenerateRequest("GET", $"{Info.Server.MANAGING_POINT}/{id}",
                Info.mainPage.AccessToken, "");

            if ((HttpStatusCode)responseDict["status"] != HttpStatusCode.OK)
            {
                Info.RefreshToken();
                responseDict = Info.GenerateRequest("GET", $"{Info.Server.MANAGING_POINT}/{id}",
                Info.mainPage.AccessToken, "");
                // MessageBox.Show("오류");
            }

            JObject responseJSON = JObject.Parse(responseDict["body"].ToString());
            JArray logs = JArray.Parse(responseJSON["point_history"].ToString());
            //Console.WriteLine(logs);

            int list_cnt = 0;
            for (int i = logs.Count - 1; i >= 0; i--)
            {

                JObject log = (JObject)logs[i];
                ListViewItem OneItem = new ListViewItem();
                OneItem.Content = new PointItem()
                {
                    //pointType = bool.Parse(log["pointType"].ToString()),
                    date = DateTime.Parse(log["date"].ToString()).ToString("yyyy-MM-dd"),
                    reason = log["reason"].ToString(),
                    point = Math.Abs((int)log["point"])
                };
                Itemss.Add(OneItem);
                PointList.ItemsSource = Itemss;

                if (bool.Parse(log["pointType"].ToString()) == true)
                {
                    ChangeRowColor(list_cnt++, Brushes.Blue);
                }
                else
                {
                    ChangeRowColor(list_cnt++, Brushes.Red);
                }
            }
            PointList.Items.Refresh();


        }

        private void ChangeRowColor(int RowIndex, SolidColorBrush NewBackground)
        {

            Itemss[RowIndex].Foreground = NewBackground;
            Itemss[RowIndex].FontSize = 18;


            PointList.Items.Refresh();

        }

        public static Dictionary<string, Brush> Keywords = new Dictionary<string, Brush>();

        private void SearchCommand_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchCommand.Text = "";
        }


        private void RuleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            if (sender == RuleList_good)
            {
                RuleList_bad.SelectedItem = null;
            }
            else
            {
                RuleList_good.SelectedItem = null;
                
            }
        }

        private void Add_Point_Button_Click(object sender, RoutedEventArgs e)
        {
            if (resultListCollection.Count == 0)
            {
                MessageBox.Show("학생을 선택해주세요.");
                return;
            }

            if (RuleList_good.SelectedIndex == -1 && RuleList_bad.SelectedIndex == -1)
            {
                MessageBox.Show("상/벌점을 선택해주세요.");
                return;
            }

            string comment = "를(을) 부여하시겠습니까?";
            if(RuleList_good.SelectedIndex != -1)
            {
                int index = RuleList_good.SelectedIndex;
                string reason = (Itemss_good[index].Content as PointItem_good).reason;
                string point = (Itemss_good[index].Content as PointItem_good).point.ToString();
                comment = reason + "(상점 " + point + "점)" + comment;
            }
            else if(RuleList_bad.SelectedIndex != -1)
            {
                int index = RuleList_bad.SelectedIndex;

                string reason = (Itemss_bad[index].Content as PointItem_bad).reason;

                string point = (Itemss_bad[index].Content as PointItem_bad).point.ToString();

                comment = reason + "(벌점 " + point + "점)" + comment;
            }
            int resultListCollection_Count = resultListCollection.Count;
            if (resultListCollection_Count < 4)
            {
                for(int i = 0; i < resultListCollection_Count; i++)
                {
                    if (i == 0)
                    {
                        comment = resultListCollection[i].Name + "에게 " + comment;
                    }
                    else
                    {
                        comment = resultListCollection[i].Name + ", " + comment;
                    }
                }
            }
            else
            {
                comment = resultListCollection_Count + "명에게 " + comment;

            }

            if (MessageBox.Show(comment, "확인", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                int id;
                if (RuleList_good.SelectedIndex != -1)
                {
                    int index = RuleList_good.SelectedIndex;
                    id = (Itemss_good[index].Content as PointItem_good).id;
                }
                else 
                {
                    int index = RuleList_bad.SelectedIndex;
                    id = (Itemss_bad[index].Content as PointItem_bad).id;
                }


                foreach (StudentListViewModel student in resultListCollection)
                {
                    var requestDict = new Dictionary<string, object>
                {
                    //                    {"id", student.ID},
                    //                    {"reason", pointDialog.PunishmentID},
                    //                    {"point", pointDialog.PunishmentScore},
                    //                    {"pointType", GoodPunishCheck.IsChecked == true ? true : false}
                    {"ruleId", id}
                };
                    var responseDict = Info.GenerateRequest("POST", $"{Info.Server.MANAGING_POINT}/{student.ID}",
                        Info.mainPage.AccessToken,
                        requestDict);

                    if ((HttpStatusCode)responseDict["status"] != HttpStatusCode.Created)
                    {
                        Info.RefreshToken();
                        responseDict = Info.GenerateRequest("POST", $"{Info.Server.MANAGING_POINT}/{student.ID}",
                        Info.mainPage.AccessToken,
                        requestDict);
                        //MessageBox.Show("오류");
                    }
                }

                resultListCollection.Clear();

                ResultList.ItemsSource = resultListCollection;
                ResultList.Items.Refresh();
            }

            MessageBox.Show("처리 완료");

            if (SearchList.SelectedItems.Count == 1)
            {
                Itemss.Clear();
                PointList.Items.Refresh();
                var target = (StudentListViewModel)SearchList.SelectedItems[0];
                SetLogData(target.ID);
            }
        }

        private void EditList_Click(object sender, RoutedEventArgs e)
        {
            PointManageDialog pointManageDialog = new PointManageDialog();
            ShowModal(pointManageDialog);
        }

        private void point_click(object sender, RoutedEventArgs e)
        {
            mark_Border_1.Visibility = Visibility.Visible;
            mark_Border_2.Visibility = Visibility.Hidden;
            mark_Border_3.Visibility = Visibility.Hidden;
            mark_Border_4.Visibility = Visibility.Hidden;
            mark_Border_5.Visibility = Visibility.Hidden;

            point_mpoint_Rectangle.Visibility = Visibility.Visible;

            SearchImage.Visibility = Visibility.Visible;
            SearchButton.Visibility = Visibility.Visible;
            SearchCommand.Visibility = Visibility.Visible;
            SearchList.Visibility = Visibility.Visible;
            mPointList.Visibility = Visibility.Hidden;
            All_Button.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Visible;
            ResultList.Visibility = Visibility.Visible;
            PointList.Visibility = Visibility.Visible;
            PointList_Rectangle.Visibility = Visibility.Visible;
            ResultList_Rectangle.Visibility = Visibility.Visible;
            RuleList_bad.Visibility = Visibility.Visible;
            RuleList_good.Visibility = Visibility.Visible;
            Point_Rectangle.Visibility = Visibility.Visible;
            Add_Point_Button.Visibility = Visibility.Visible;
            Edit_List_Button.Visibility = Visibility.Visible;
            Rule_Border.Visibility = Visibility.Visible;
            Good_Point_Button.Visibility = Visibility.Visible;
            Bad_Point_Button.Visibility = Visibility.Visible;

            Download_1_Button.Visibility = Visibility.Hidden;
            Download_2_Button.Visibility = Visibility.Hidden;
            Download_3_Button.Visibility = Visibility.Hidden;
            Download_4_Button.Visibility = Visibility.Hidden;
            Download_5_Button.Visibility = Visibility.Hidden;

            Grad_Border.Visibility = Visibility.Visible;
            All_Grade_Button.Visibility = Visibility.Visible;
            First_Grade_Button.Visibility = Visibility.Visible;
            Second_Grade_Button.Visibility = Visibility.Visible;
            Third_Grade_Button.Visibility = Visibility.Visible;

            notice_ChatList_Rectangle.Visibility = Visibility.Hidden;
            notice_chat_Rectangle.Visibility = Visibility.Hidden;
            chatList.Visibility = Visibility.Hidden;
            Point_Rectangle.Visibility = Visibility.Hidden;
            chat_input_TextBox.Visibility = Visibility.Hidden;
            chat_send_Button.Visibility = Visibility.Hidden;
            chat_1_Border.Visibility = Visibility.Hidden;
            chat_2_Border.Visibility = Visibility.Hidden;
            chat_StackPanel.Visibility = Visibility.Hidden;
            chat_name_Label.Visibility = Visibility.Hidden;

            First_Grade_Button.Content = "1학년";
            Second_Grade_Button.Content = "2학년";
            Third_Grade_Button.Content = "3학년";


            var bc = new BrushConverter();

            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");

            if (filter == "전체")
            {
                All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
                All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            }
            else if(filter == "1학년")
            {
                First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
                First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            }else if(filter == "2학년")
            {
                Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
                Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            }
            else
            {
                Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
                Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            }
        }

        private void mpoint_click(object sender, RoutedEventArgs e)
        {
            mark_Border_1.Visibility = Visibility.Hidden;
            mark_Border_2.Visibility = Visibility.Visible;
            mark_Border_3.Visibility = Visibility.Hidden;
            mark_Border_4.Visibility = Visibility.Hidden;
            mark_Border_5.Visibility = Visibility.Hidden;
            
            mpoint_Update();

            point_mpoint_Rectangle.Visibility = Visibility.Visible;

            SearchImage.Visibility = Visibility.Hidden;
            SearchButton.Visibility = Visibility.Hidden;
            SearchCommand.Visibility = Visibility.Hidden;
            SearchList.Visibility = Visibility.Hidden;
            mPointList.Visibility = Visibility.Visible;
            All_Button.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            ResultList.Visibility = Visibility.Hidden;
            PointList.Visibility = Visibility.Hidden;
            PointList_Rectangle.Visibility = Visibility.Hidden;
            ResultList_Rectangle.Visibility = Visibility.Hidden;
            RuleList_bad.Visibility = Visibility.Hidden;
            RuleList_good.Visibility = Visibility.Hidden;
            Point_Rectangle.Visibility = Visibility.Hidden;
            Add_Point_Button.Visibility = Visibility.Hidden;
            Edit_List_Button.Visibility = Visibility.Hidden;
            Rule_Border.Visibility = Visibility.Hidden;
            Good_Point_Button.Visibility = Visibility.Hidden;
            Bad_Point_Button.Visibility = Visibility.Hidden;

            Download_1_Button.Visibility = Visibility.Hidden;
            Download_2_Button.Visibility = Visibility.Hidden;
            Download_3_Button.Visibility = Visibility.Hidden;
            Download_4_Button.Visibility = Visibility.Hidden;
            Download_5_Button.Visibility = Visibility.Hidden;

            Grad_Border.Visibility = Visibility.Visible;
            All_Grade_Button.Visibility = Visibility.Visible;
            First_Grade_Button.Visibility = Visibility.Visible;
            Second_Grade_Button.Visibility = Visibility.Visible;
            Third_Grade_Button.Visibility = Visibility.Visible;

            Notice_1_Rectangle.Visibility = Visibility.Hidden;
            Notice_2_Rectangle.Visibility = Visibility.Hidden;
            NoticeList.Visibility = Visibility.Hidden;
            notice_title.Visibility = Visibility.Hidden;
            notice_content.Visibility = Visibility.Hidden;
            notice_Delete_Button.Visibility = Visibility.Hidden;
            notice_Edit_Button.Visibility = Visibility.Hidden;

            notice_ChatList_Rectangle.Visibility = Visibility.Hidden;
            notice_chat_Rectangle.Visibility = Visibility.Hidden;

            chatList.Visibility = Visibility.Hidden;
            Point_Rectangle.Visibility = Visibility.Hidden;
            chat_input_TextBox.Visibility = Visibility.Hidden;
            chat_send_Button.Visibility = Visibility.Hidden;
            chat_1_Border.Visibility = Visibility.Hidden;
            chat_2_Border.Visibility = Visibility.Hidden;
            chat_StackPanel.Visibility = Visibility.Hidden;
            chat_name_Label.Visibility = Visibility.Hidden;

            var bc = new BrushConverter();

            All_Grade_Button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            First_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Second_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            Third_Grade_Button.Background = (Brush)bc.ConvertFrom("#FF2F5FD2");
            All_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FF2F5FD2");
            First_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Second_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Third_Grade_Button.Foreground = (Brush)bc.ConvertFrom("#FFFFFFFF");

            First_Grade_Button.Content = "1단계";
            Second_Grade_Button.Content = "2단계";
            Third_Grade_Button.Content = "3단계";
        }

        private void download_click(object sender, RoutedEventArgs e)
        {

            mark_Border_1.Visibility = Visibility.Hidden;
            mark_Border_2.Visibility = Visibility.Hidden;
            mark_Border_3.Visibility = Visibility.Visible;
            mark_Border_4.Visibility = Visibility.Hidden;
            mark_Border_5.Visibility = Visibility.Hidden;

            point_mpoint_Rectangle.Visibility = Visibility.Hidden;
            SearchImage.Visibility = Visibility.Hidden;
            SearchButton.Visibility = Visibility.Hidden;
            SearchCommand.Visibility = Visibility.Hidden;
            SearchList.Visibility = Visibility.Hidden;
            mPointList.Visibility = Visibility.Hidden;
            All_Button.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            ResultList.Visibility = Visibility.Hidden;
            PointList.Visibility = Visibility.Hidden;
            PointList_Rectangle.Visibility = Visibility.Hidden;
            ResultList_Rectangle.Visibility = Visibility.Hidden;
            RuleList_bad.Visibility = Visibility.Hidden;
            RuleList_good.Visibility = Visibility.Hidden;
            Point_Rectangle.Visibility = Visibility.Hidden;
            Add_Point_Button.Visibility = Visibility.Hidden;
            Edit_List_Button.Visibility = Visibility.Hidden;
            Rule_Border.Visibility = Visibility.Hidden;
            Good_Point_Button.Visibility = Visibility.Hidden;
            Bad_Point_Button.Visibility = Visibility.Hidden;

            Download_1_Button.Visibility = Visibility.Visible;
            Download_2_Button.Visibility = Visibility.Visible;
            Download_3_Button.Visibility = Visibility.Visible;
            Download_4_Button.Visibility = Visibility.Visible;
            Download_5_Button.Visibility = Visibility.Visible;

            Grad_Border.Visibility = Visibility.Hidden;
            All_Grade_Button.Visibility = Visibility.Hidden;
            First_Grade_Button.Visibility = Visibility.Hidden;
            Second_Grade_Button.Visibility = Visibility.Hidden;
            Third_Grade_Button.Visibility = Visibility.Hidden;

            Notice_1_Rectangle.Visibility = Visibility.Hidden;
            Notice_2_Rectangle.Visibility = Visibility.Hidden;
            NoticeList.Visibility = Visibility.Hidden;
            notice_title.Visibility = Visibility.Hidden;
            notice_content.Visibility = Visibility.Hidden;
            notice_Delete_Button.Visibility = Visibility.Hidden;
            notice_Edit_Button.Visibility = Visibility.Hidden;

            notice_ChatList_Rectangle.Visibility = Visibility.Hidden;
            notice_chat_Rectangle.Visibility = Visibility.Hidden;

            chatList.Visibility = Visibility.Hidden;
            Point_Rectangle.Visibility = Visibility.Hidden;
            chat_input_TextBox.Visibility = Visibility.Hidden;
            chat_send_Button.Visibility = Visibility.Hidden;
            chat_1_Border.Visibility = Visibility.Hidden;
            chat_2_Border.Visibility = Visibility.Hidden;
            chat_StackPanel.Visibility = Visibility.Hidden;
            chat_name_Label.Visibility = Visibility.Hidden;
        }

        private void notice_click(object sender, RoutedEventArgs e)
        {

            mark_Border_1.Visibility = Visibility.Hidden;
            mark_Border_2.Visibility = Visibility.Hidden;
            mark_Border_3.Visibility = Visibility.Hidden;
            mark_Border_4.Visibility = Visibility.Visible;
            mark_Border_5.Visibility = Visibility.Hidden;

            point_mpoint_Rectangle.Visibility = Visibility.Hidden;
            SearchImage.Visibility = Visibility.Hidden;
            SearchButton.Visibility = Visibility.Hidden;
            SearchCommand.Visibility = Visibility.Hidden;
            SearchList.Visibility = Visibility.Hidden;
            mPointList.Visibility = Visibility.Hidden;
            All_Button.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            ResultList.Visibility = Visibility.Hidden;
            PointList.Visibility = Visibility.Hidden;
            PointList_Rectangle.Visibility = Visibility.Hidden;
            ResultList_Rectangle.Visibility = Visibility.Hidden;
            RuleList_bad.Visibility = Visibility.Hidden;
            RuleList_good.Visibility = Visibility.Hidden;
            Point_Rectangle.Visibility = Visibility.Hidden;
            Add_Point_Button.Visibility = Visibility.Hidden;
            Edit_List_Button.Visibility = Visibility.Hidden;
            Rule_Border.Visibility = Visibility.Hidden;
            Good_Point_Button.Visibility = Visibility.Hidden;
            Bad_Point_Button.Visibility = Visibility.Hidden;

            Download_1_Button.Visibility = Visibility.Hidden;
            Download_2_Button.Visibility = Visibility.Hidden;
            Download_3_Button.Visibility = Visibility.Hidden;
            Download_4_Button.Visibility = Visibility.Hidden;
            Download_5_Button.Visibility = Visibility.Hidden;

            Grad_Border.Visibility = Visibility.Hidden;
            All_Grade_Button.Visibility = Visibility.Hidden;
            First_Grade_Button.Visibility = Visibility.Hidden;
            Second_Grade_Button.Visibility = Visibility.Hidden;
            Third_Grade_Button.Visibility = Visibility.Hidden;

            Notice_1_Rectangle.Visibility = Visibility.Visible;
            Notice_2_Rectangle.Visibility = Visibility.Visible;
            NoticeList.Visibility = Visibility.Visible;
            notice_title.Visibility = Visibility.Visible;
            notice_content.Visibility = Visibility.Visible;
            notice_Delete_Button.Visibility = Visibility.Visible;
            notice_Edit_Button.Visibility = Visibility.Visible;

            notice_ChatList_Rectangle.Visibility = Visibility.Hidden;
            notice_chat_Rectangle.Visibility = Visibility.Hidden;
            chatList.Visibility = Visibility.Hidden;
            Point_Rectangle.Visibility = Visibility.Hidden;
            chat_input_TextBox.Visibility = Visibility.Hidden;
            chat_send_Button.Visibility = Visibility.Hidden;
            chat_1_Border.Visibility = Visibility.Hidden;
            chat_2_Border.Visibility = Visibility.Hidden;
            chat_StackPanel.Visibility = Visibility.Hidden;
            chat_name_Label.Visibility = Visibility.Hidden;
        }

        private void chat_click(object sender, RoutedEventArgs e)
        {

            mark_Border_1.Visibility = Visibility.Hidden;
            mark_Border_2.Visibility = Visibility.Hidden;
            mark_Border_3.Visibility = Visibility.Hidden;
            mark_Border_4.Visibility = Visibility.Hidden;
            mark_Border_5.Visibility = Visibility.Visible;

            point_mpoint_Rectangle.Visibility = Visibility.Hidden;
            SearchImage.Visibility = Visibility.Hidden;
            SearchButton.Visibility = Visibility.Hidden;
            SearchCommand.Visibility = Visibility.Hidden;
            SearchList.Visibility = Visibility.Hidden;
            mPointList.Visibility = Visibility.Hidden;
            All_Button.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            ResultList.Visibility = Visibility.Hidden;
            PointList.Visibility = Visibility.Hidden;
            PointList_Rectangle.Visibility = Visibility.Hidden;
            ResultList_Rectangle.Visibility = Visibility.Hidden;
            RuleList_bad.Visibility = Visibility.Hidden;
            RuleList_good.Visibility = Visibility.Hidden;
            Point_Rectangle.Visibility = Visibility.Hidden;
            Add_Point_Button.Visibility = Visibility.Hidden;
            Edit_List_Button.Visibility = Visibility.Hidden;
            Rule_Border.Visibility = Visibility.Hidden;
            Good_Point_Button.Visibility = Visibility.Hidden;
            Bad_Point_Button.Visibility = Visibility.Hidden;

            Download_1_Button.Visibility = Visibility.Hidden;
            Download_2_Button.Visibility = Visibility.Hidden;
            Download_3_Button.Visibility = Visibility.Hidden;
            Download_4_Button.Visibility = Visibility.Hidden;
            Download_5_Button.Visibility = Visibility.Hidden;

            Grad_Border.Visibility = Visibility.Hidden;
            All_Grade_Button.Visibility = Visibility.Hidden;
            First_Grade_Button.Visibility = Visibility.Hidden;
            Second_Grade_Button.Visibility = Visibility.Hidden;
            Third_Grade_Button.Visibility = Visibility.Hidden;

            Notice_1_Rectangle.Visibility = Visibility.Hidden;
            Notice_2_Rectangle.Visibility = Visibility.Hidden;
            NoticeList.Visibility = Visibility.Hidden;
            notice_title.Visibility = Visibility.Hidden;
            notice_content.Visibility = Visibility.Hidden;
            notice_Delete_Button.Visibility = Visibility.Hidden;
            notice_Edit_Button.Visibility = Visibility.Hidden;

            notice_ChatList_Rectangle.Visibility = Visibility.Visible;
            notice_chat_Rectangle.Visibility = Visibility.Visible;
            chatList.Visibility = Visibility.Visible;
            Point_Rectangle.Visibility = Visibility.Visible;
            chat_input_TextBox.Visibility = Visibility.Visible;
            chat_send_Button.Visibility = Visibility.Visible;
            chat_1_Border.Visibility = Visibility.Visible;
            chat_2_Border.Visibility = Visibility.Visible;
            chat_StackPanel.Visibility = Visibility.Visible;
            chat_name_Label.Visibility = Visibility.Visible;
        }
        private T mpoint_GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return mpoint_GetAncestorOfType<T>((FrameworkElement)parent);

            return (T)parent;
        }

        private void StudentList_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listView = sender as ListView;
            var gridView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 18;

            double[] columnRatio =
            {
                0.25,
                0.25,
                0.25,
                0.25
            };

            foreach (var element in gridView.Columns)
                element.Width = workingWidth * columnRatio[gridView.Columns.IndexOf(element)];
        }

        private void FilteringStudent(int step)
        {
            mpoint_listviewCollection.Clear();
            foreach (JObject student in mpoint_studentList)
            {
                int currentStep = student["penaltyLevel"].Type == JTokenType.Null
                    ? 0
                    : (int)student["penaltyLevel"];

                if (currentStep == step && (bool)student["penaltyTrainingStatus"] == true)
                {
                    mpoint_listviewCollection.Add(new ViewModel.StudentListViewModel(
                        id: student["id"].ToString(),
                        classNumber: student["number"].ToString(),
                        name: student["name"].ToString(),
                        goodPoint: student["goodPoint"].Type == JTokenType.Null ? 0 : (int)student["goodPoint"],
                        badPoint: student["badPoint"].Type == JTokenType.Null ? 0 : (int)student["badPoint"],
                        penaltyTrainingStaus: bool.Parse(student["penaltyTrainingStatus"].ToString()),
                        penaltyLevel: bool.Parse(student["penaltyTrainingStatus"].ToString()) == true
                            ? Info.ParseStatus((int)student["penaltyLevel"])
                            : " ",
                        isSelected: false
                    ));
                }

                /*{
                    listviewCollection.Add(new ViewModel.StudentListViewModel(
                        id: student["id"].ToString(),
                        classNumber: student["number"].ToString(),
                        name: student["name"].ToString(),
                        goodPoint: student["goodPoint"].Type == JTokenType.Null ? 0 : (int)student["goodPoint"],
                        badPoint: student["badPoint"].Type == JTokenType.Null ? 0 : (int)student["badPoint"],
                        penaltyTrainingStaus: bool.Parse(student["penaltySrainingStatus"].ToString()),
                        penaltyLevel: Info.ParseStatus(currentStep),
                        isSelected: false
                    ));
                }*/
            }
        }

        private void Offset_Click(object sender, RoutedEventArgs e)
        {
            var target = (StudentListViewModel)GetAncestorOfType<ListViewItem>(sender as Button).DataContext;
            Info.RefreshToken();
            string strTarget = target.PenaltyLevel;
            string strTmp = Regex.Replace(strTarget, @"\D", "");
            int nTmp = int.Parse(strTmp);

            var requestDict = new Dictionary<string, object>
            {
                 { "studentId", target.ID },
                 { "penaltyLevel", nTmp }
            };
            Console.WriteLine(requestDict);
            var responseDict = Info.GenerateRequest("POST", Info.Server.MANAGING_PENALTY, Info.mainPage.AccessToken, requestDict);
            Console.WriteLine(responseDict["status"]);
            if ((HttpStatusCode)responseDict["status"] != HttpStatusCode.OK)
            {
                MessageBox.Show("오류입니다. 방송으로 유동근을 불러주세요.");
            }
            MessageBox.Show("상쇄 성공!");
            mpoint_listviewCollection.Clear();
            mpoint_Update();
        }

        private void mpoint_Update()
        {
            mpoint_listviewCollection.Clear();
            var responseDict = Info.GenerateRequest("GET", Info.Server.MANAGING_STUDENT, Info.mainPage.AccessToken, "");

            if ((HttpStatusCode)responseDict["status"] != HttpStatusCode.OK)
            {
                Info.RefreshToken();
                responseDict = Info.GenerateRequest("GET", Info.Server.MANAGING_STUDENT, Info.mainPage.AccessToken, "");
                //MessageBox.Show("오류");
            }
            Console.WriteLine("aa");

            JObject responseJSON = JObject.Parse(responseDict["body"].ToString());
            mpoint_studentList = JArray.Parse(responseJSON["studentList"].ToString());

            Console.WriteLine("aa");
            foreach (JObject student in mpoint_studentList)
            {
                int currentStep = student["penaltyLevel"].Type == JTokenType.Null
                    ? 0
                    : (int)student["penaltyLevel"];

                if (mpoint_filter != "전체" && Info.ParseStatus(currentStep).Equals(mpoint_filter))
                {
                    continue;
                }

                if (currentStep >= 1 && (bool)student["penaltyTrainingStatus"] == true)
                {
                    mpoint_listviewCollection.Add(new ViewModel.StudentListViewModel(
                        id: student["id"].ToString(),
                        classNumber: student["number"].ToString(),
                        name: student["name"].ToString(),
                        goodPoint: student["goodPoint"].Type == JTokenType.Null ? 0 : (int)student["goodPoint"],
                        badPoint: student["badPoint"].Type == JTokenType.Null ? 0 : (int)student["badPoint"],
                        penaltyTrainingStaus: bool.Parse(student["penaltyTrainingStatus"].ToString()),
                        penaltyLevel: bool.Parse(student["penaltyTrainingStatus"].ToString()) == true
                            ? Info.ParseStatus((int)student["penaltyLevel"])
                            : " ",
                        isSelected: false
                    ));
                }
            }
        }
    }
}
public class PointItem
{
    public bool pointType { get; set; }
    public string date { get; set; }
    public int point { get; set; }

    public string reason { get; set; }

}

public class PointItem_good
{
    public string reason { get; set; }
    public int point { get; set; }
    public int id { get; set; }
}

public class PointItem_bad
{
    public string reason { get; set; }
    public int point { get; set; }
    public int id { get; set; }
}



/* private void HideAnimation(Panel target)
        {
            var Duration = new Duration(new TimeSpan(0, 0, 0, 0, 600));

            Storyboard HideStoryBoard = new Storyboard();

            DoubleAnimation FadeOutAnimation = new DoubleAnimation(0, Duration)
            {
                EasingFunction = new QuadraticEase()
            };

            Storyboard.SetTargetProperty(FadeOutAnimation, new PropertyPath(OpacityProperty));

            Storyboard.SetTarget(FadeOutAnimation, target);

            ThicknessAnimation ShiftLeftAnimation =
            new ThicknessAnimation(new Thickness(0, 0, 200, target.Margin.Bottom), Duration)
            {
                EasingFunction = new QuadraticEase()
            };

            Storyboard.SetTargetProperty(ShiftLeftAnimation, new PropertyPath(MarginProperty));

            Storyboard.SetTarget(ShiftLeftAnimation, target);

            HideStoryBoard.Children.Add(FadeOutAnimation);
            HideStoryBoard.Children.Add(ShiftLeftAnimation);

            target.Visibility = Visibility.Hidden;
            HideStoryBoard.Begin();
        }

        private void ShowAnimation(Panel target)
        {
            var Duration = new Duration(new TimeSpan(0, 0, 0, 0, 600));

            Storyboard HideStoryBoard = new Storyboard();

            DoubleAnimation FadeInAnimation = new DoubleAnimation(1, Duration)
            {
                EasingFunction = new QuadraticEase()
            };

            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTarget(FadeInAnimation, target);

            ThicknessAnimation ShiftLeftAnimation =
                new ThicknessAnimation(new Thickness(0, 0, 0, target.Margin.Bottom), Duration)
                {
                    EasingFunction = new QuadraticEase()
                };

            Storyboard.SetTargetProperty(ShiftLeftAnimation, new PropertyPath(MarginProperty));
            Storyboard.SetTarget(ShiftLeftAnimation, target);

            HideStoryBoard.Children.Add(FadeInAnimation);
            HideStoryBoard.Children.Add(ShiftLeftAnimation);

            target.Visibility = Visibility.Visible;
            HideStoryBoard.Begin();
        } */
