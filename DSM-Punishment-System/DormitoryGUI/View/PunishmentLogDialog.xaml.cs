using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DormitoryGUI.View
{
    /// <summary>
    /// PunishmentLogDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PunishmentLogDialog : Window
    {
        private string studentID;

        public PunishmentLogDialog(string id, string name, string classNumber, int goodPoint, int badPoint,
            string currentStep)
        {
            InitializeComponent();

            StudentName.Content = name;
            ClassNumber.Content = classNumber;

            TotalGoodPoint.Content = goodPoint;
            TotalBadPoint.Content = badPoint;
            TotalPunishStep.Content = currentStep;

            studentID = id;
            SetLogData(id);
        }

        private void SetLogData(string id)
        {
            var responseDict = Info.GenerateRequest("GET", $"{Info.Server.MANAGING_POINT}/{id}",
                Info.mainPage.AccessToken, "");
            if ((HttpStatusCode) responseDict["status"] != HttpStatusCode.OK)
            {
                Info.RefreshToken();
                responseDict = Info.GenerateRequest("GET", $"{Info.Server.MANAGING_POINT}/{id}",
                Info.mainPage.AccessToken, "");
               // MessageBox.Show("오류");
            }
            JObject responseJSON = JObject.Parse(responseDict["body"].ToString());
            JArray logs = JArray.Parse(responseJSON["point_history"].ToString());
            Console.WriteLine(logs);
            Timeline.Children.Clear();

            for (int i = logs.Count - 1; i >= 0; i--)
            {
                JObject log = (JObject) logs[i];

                TimelineBlock timelineBlock =
                    new TimelineBlock
                    (
                        isGood: bool.Parse(log["pointType"].ToString()),
                        createTime: DateTime.Parse(log["date"].ToString()).ToString("yyyy-MM-dd"),
                        pointValue: Math.Abs((int) log["point"]).ToString(),
                        pointCause: log["reason"].ToString()
                    );
                timelineBlock.RemoveButton.Click +=
                    (sender, e) =>
                    {
                        var requestDict = new Dictionary<string, object>
                        {                            
                            {"historyId", log["id"]}
                        };
                        responseDict = Info.GenerateRequest("DELETE", $"{Info.Server.MANAGING_POINT}/{studentID}",
                            Info.mainPage.AccessToken, requestDict);

                        if ((HttpStatusCode) responseDict["status"] != HttpStatusCode.OK)
                        {
                            Info.RefreshToken();
                            responseDict = Info.GenerateRequest("DELETE", $"{Info.Server.MANAGING_POINT}/{studentID}",
                            Info.mainPage.AccessToken, requestDict);
                            //MessageBox.Show("오류");
                        }

                        MessageBox.Show("상벌점 내역 삭제 성공");
//                        int point = (int) timelineBlock.PointValue.Content;
//                        TotalGoodPoint.Content = ((int) TotalGoodPoint.Content - point).ToString();
                        SetLogData(id);
                    };

                Timeline.Children.Add(timelineBlock);
            }
        }
    }
}