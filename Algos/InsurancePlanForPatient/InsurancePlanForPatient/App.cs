using CsvUtility;

namespace InsurancePlanForPatient;

public class App
{
    public async Task RunAsync()
    {
        var path = Directory.GetCurrentDirectory();

        var patients = CsvUtilityHelper.GetDataFromInputCsv<Patients>(path + "\\PatientMeds.csv");
        var insurancePlans = CsvUtilityHelper.GetDataFromInputCsv<InsurancePlans>(path + "\\InsurancePlans.csv");

        await Task.WhenAll(patients, insurancePlans);
        var insurancePlanForPatientDatas = GetEfficientInsurancePlanForPatient(patients.Result, insurancePlans.Result);

        var outputPath = Path.Combine(path, "EfficientInsurancePlans.csv");
        CsvUtilityHelper.WriteOutPutCsv<InsurancePlanForPatientData>(outputPath, insurancePlanForPatientDatas.ToArray());
    }

    private static List<InsurancePlanForPatientData> GetEfficientInsurancePlanForPatient(IEnumerable<Patients> patients,
        IEnumerable<InsurancePlans> insurancePlans)
    {
        var insurancePlanForPatient = new List<InsurancePlanForPatientData>();
        var patientGroups = patients.GroupBy(x => x.PatientId).ToList();
        foreach (var patientGroup in patientGroups)
        {
            var patientId = patientGroup.Key;
            long totalCost = 0;
            var planDetails = new Dictionary<string, long>();

            foreach (var patient in patientGroup)
            {
                var numberOfDays = (DateTime.Now - patient.StartDate).Days;

                var totalDaysOfSupply = (long)numberOfDays / patient.DaysSupply;

                totalCost += totalDaysOfSupply * patient.Cost;
            }

            foreach (var plan in insurancePlans)
            {
                long totalPayable = 0;
                var minimumeExpense = plan.Premium + plan.MaximumOutOfPocketExpenses;
                if (plan.Deductible < totalCost)
                {
                    var payableAmount = totalCost * plan.Coinsurance / 100;
                    totalPayable = minimumeExpense + payableAmount;

                    planDetails.Add(plan.InsurancePlanName, totalPayable);
                }
            }

            var efficientPlan = planDetails.OrderBy(x => x.Value).FirstOrDefault();
            insurancePlanForPatient.Add(new InsurancePlanForPatientData
            {
                PatientId = patientId,
                Cost = efficientPlan.Value,
                RecommendedInsurancePlan = efficientPlan.Key
            });
            Console.WriteLine(patientId + " has total cost as " + totalCost);
        }

        return insurancePlanForPatient;
    }
}