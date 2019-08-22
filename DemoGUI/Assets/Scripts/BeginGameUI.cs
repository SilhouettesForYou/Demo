using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class BeginGameUI : BaseUI
    {

        private static bool isLogin = false;
        private string username;
        private string password;

        GameObject loginOrRegisterPanel;


        // Start is called before the first frame update
        void Awake()
        {
            //定义窗口性质 (默认数值)
            curUIType.type = UIFormType.Normal;
            curUIType.mode = UIFormShowMode.Normal;
            curUIType.lucencyType = UIFormLucencyType.Lucency;

            loginOrRegisterPanel = GameObject.Find("LoginOrRegisterPanel");

            //注册按钮
            RegisterBtnOnClick("BeginGameBtn", StartGame);
            RegisterBtnOnClick("LoadProgressBtn", LoadProgress);
            RegisterBtnOnClick("AboutBtn", About);

            RegisterBtnOnClick("HomeBtn", Home);
            RegisterBtnOnClick("SettingsBtn", Settings);
            RegisterBtnOnClick("TrophyBtn", Trophy);

        }

        public void StartGame(GameObject go)
        {
            //前台或者后台检查用户名称与密码
            Debug.Log("开始游戏");
            //如果成功，则登录下一个窗口
            OpenUI(UINameConst.SelectLevelUI);
        }

        public void LoadProgress(GameObject go)
        {
            Debug.Log("载入进度");
        }

        public void About(GameObject go)
        {
            Debug.Log("关于");
        }

        public void Home(GameObject go)
        {
            if (isLogin == false)
            {
                Debug.Log("Didn't detect registration or login information. Begin logining or registering...");
                loginOrRegisterPanel.transform.localScale = new Vector3(1, 1, 1);   // transform login or register panel to visible
                Button registerBtn = loginOrRegisterPanel.GetComponentInChildren<Button>(); // find the login or register button

                RegisterBtnOnClick(registerBtn.transform, LoginOrRegister);

                // obtain the input text in name filed or passeord field
                InputField[] inputFields = loginOrRegisterPanel.GetComponentsInChildren<InputField>();
                for (int i = 0; i < inputFields.Length; i++)
                {
                    if (inputFields[i].name == "UserName")
                    {
                        username = inputFields[i].text;
                    }
                    if (inputFields[i].name == "Password")
                    {
                        password = inputFields[i].text;
                    }
                }
            }
            else if (isLogin == true)
            {
                OpenUI(UINameConst.HomeUI);
                Debug.Log("Popup personal information...");
            }
        }

        public void Settings(GameObject go)
        {

        }

        public void Trophy(GameObject go)
        {

        }

        public void LoginOrRegister(GameObject go)
        {
            Debug.Log("Request login or register information from server...");
            StartCoroutine(SendData());
        }

        private IEnumerator SendData()
        {
            yield return new WaitForSeconds(1.0f);
            //string url = "https://192.168.1.101/";
            //Encoding encoding = Encoding.GetEncoding("utf-8");
            //IDictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("name", username);
            //parameters.Add("password", password);
            //HttpWebResponse response = HTTPSRequest.CreatePostHttpResponse(url, parameters, encoding);
            //Stream stream = response.GetResponseStream();
            //StreamReader sr = new StreamReader(stream);
            //string text = sr.ReadToEnd();
            //Debug.Log(text);
            Debug.Log("Login");
            loginOrRegisterPanel.transform.localScale = new Vector3(0, 0, 0);
            isLogin = true;

            //yield break;
        }
    }
}
