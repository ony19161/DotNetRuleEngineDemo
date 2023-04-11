using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Generic;
using LearnDotNetCore.Models;
using Newtonsoft.Json;
using RulesEngine.Models;
using LearnDotNetCore.Services.Validation;
using LearnDotNetCore.Models.Request;
using System.Threading.Tasks;

namespace LearnDotNetCore
{
    internal class Program
    {
        private static IConfiguration _iconfiguration;
        static void Main(string[] args)
        {
            /*List<Workflow> workflows = new List<Workflow>();
            Workflow workflow = new Workflow();
            workflow.WorkflowName = "Username";
            List<RulesEngine.Models.Rule> rules = new List<RulesEngine.Models.Rule>();

            RulesEngine.Models.Rule rule = new RulesEngine.Models.Rule();
            rule.RuleName = "Test Rule";
            rule.SuccessEvent = "Count is within tolerance.";
            rule.ErrorMessage = "Over expected.";
            rule.Expression = "count < 3";
            rule.RuleExpressionType = RuleExpressionType.LambdaExpression;
            rules.Add(rule);

            workflow.Rules = rules;
            workflows.Add(workflow);

            Console.WriteLine(JsonConvert.SerializeObject(workflows));*/

            TestRuleEngineValidation(new UserRegister
            {
                Username = ""
            });


            Console.WriteLine("Press any key to stop.");
            Console.ReadKey();
        }

        static async Task TestRuleEngineValidation(UserRegister userInfo)
        {
            var rulesFilePath = Path.Combine(Environment.CurrentDirectory, "rules.json");

            var workflowList = JsonConvert.DeserializeObject<List<Workflow>>(File.ReadAllText(rulesFilePath));
            var reSettingsWithCustomTypes = new ReSettings { CustomTypes = new Type[] { typeof(Utils) } };
            var ruleEngine = new RulesEngine.RulesEngine(workflowList.ToArray(), reSettingsWithCustomTypes);
            var rp1 = new RuleParameter("userInfo", userInfo);
            List<RuleResultTree> resultList = await ruleEngine.ExecuteAllRulesAsync("Username", rp1);

            foreach (var item in resultList)
            {
                await Console.Out.WriteLineAsync(item.IsSuccess.ToString() + " : " + item.ExceptionMessage);
            }
        }


        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }

        static void LoadErrorCodes()
        {
            Dictionary<string, ErrorInfo> errorCodes = new Dictionary<string, ErrorInfo>();

        }

        static void TestDbConnection()
        {
            var connectionString = _iconfiguration.GetConnectionString("Secondary");

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Departments]", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
