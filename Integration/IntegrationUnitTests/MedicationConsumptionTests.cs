using System;
using System.Collections.Generic;
using System.Linq;
using Integration.Partnership.Model;
using Integration.Partnership.Repository;
using Integration.Partnership.Service;
using Integration.Shared.Model;
using IntegrationUnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace IntegrationUnitTests
{
    public class MedicationConsumptionTests : BaseTest
    {
        
        public MedicationConsumptionTests(BaseFixture fixture) : base(fixture)
        {
            Context.Medicines.RemoveRange(Context.Medicines);
            Context.SaveChanges();
            MakeReceipts();
            Context.SaveChanges();
        }

        [Theory]
        [MemberData(nameof(GetReceiptsData))]
        public void Get_receipts_in_time_range(TimeRange timeRange, int shouldBe)
        {
            var receipts = UoW.GetRepository<IReceiptReadRepository>().GetReceiptLogsInTimeRange(timeRange);
            receipts.Count().ShouldBe(shouldBe);
        }

        public static IEnumerable<object[]> GetReceiptsData()
        {
            List<object[]> retVal = new()
            {
                new object[]
                {new TimeRange (new DateTime(2021, 9, 1), new DateTime(2021, 10, 1)), 3},
                new object[]
                {new TimeRange (new DateTime(2020, 9, 1), new DateTime(2021, 11, 1)), 5},
                new object[]
                {new TimeRange (new DateTime(2021, 10, 1),new DateTime(2021, 11, 1)), 1},
                new object[]
                {new TimeRange (new DateTime(2021, 11, 1), new DateTime(2021, 12, 1)), 1}
            };
            return retVal;
        }

        [Fact]
        public void Calculate_medicine_consumptions()
        {
            var receipts = UoW.GetRepository<IReceiptReadRepository>().GetAll().Include(x => x.Medicine);
            IEnumerable<MedicineConsumption> medicineConsumptions = MedicineConsumptionCalculationMicroService.CalculateMedicineConsumptions(receipts);
            medicineConsumptions.Count().ShouldBe(3);
        }
        [Theory]
        [MemberData(nameof(GetTimeRanges))]
        public void Create_medication_report(TimeRange timeRange, int shouldBe)
        {
            MedicineConsumptionMasterService service = new(UoW);
            MedicineConsumptionReport report = service.CreateConsumptionReportInTimeRange(timeRange);
            report.MedicineConsumptions.Count().ShouldBe(shouldBe);
        }
        public static IEnumerable<object[]> GetTimeRanges()
        {
            TimeRange september = new (new DateTime(2021, 9, 1), new DateTime(2021, 10, 1) );
            TimeRange november = new  (new DateTime(2021, 11, 1), new DateTime(2021, 12, 1));
            TimeRange december = new (new DateTime(2021, 12, 1), new DateTime(2022, 1, 1));
            List<object[]> retVal = new()
            {
                new object[] { september, 3 },
                new object[] { november, 1 },
                new object[] { december, 2 }
            };
            return retVal;
        }
        private void MakeReceipts()
        {
            Medicine aspirin = new() { Id = 1, Name = "Aspirin" };
            Medicine probiotik = new() { Id = 2, Name = "Probiotik" };
            Medicine brufen = new() { Id = 3, Name = "Brufen" };
            Context.Medicines.Add(aspirin);
            Context.Medicines.Add(probiotik);
            Context.Medicines.Add(brufen);
            Receipt receipt1 = new()
            {
                Id = 1,
                ReceiptDate = new DateTime(2021, 9, 30),
                Medicine = brufen,
                AmountSpent = 4
            };
            Receipt receipt2 = new()
            {
                Id = 2,
                ReceiptDate = new DateTime(2021, 5, 19),
                Medicine = probiotik,
                AmountSpent = 2
            };
            Receipt receipt3 = new()
            {
                Id = 3,
                ReceiptDate = new DateTime(2021, 9, 19),
                Medicine = probiotik,
                AmountSpent = 4
            };
            Receipt receipt4 = new()
            {
                Id = 4,
                ReceiptDate = new DateTime(2021, 10, 19),
                Medicine = brufen,
                AmountSpent = 1
            };
            Receipt receipt5 = new()
            {
                Id = 5,
                ReceiptDate = new DateTime(2021, 9, 5),
                Medicine = aspirin,
                AmountSpent = 2
            };
            Receipt receipt6 = new Receipt
            {
                Id = 6,
                ReceiptDate = new DateTime(2021, 11, 5),
                Medicine = aspirin,
                AmountSpent = 5
            };
            Receipt receipt7 = new()
            {
                Id = 7,
                ReceiptDate = new DateTime(2021, 12, 5),
                Medicine = brufen,
                AmountSpent = 8
            };
            Receipt receipt8 = new()
            {
                Id = 8,
                ReceiptDate = new DateTime(2021, 12, 6),
                Medicine = aspirin,
                AmountSpent = 12
            };
            Context.Receipts.Add(receipt1);
            Context.Receipts.Add(receipt2);
            Context.Receipts.Add(receipt3);
            Context.Receipts.Add(receipt4);
            Context.Receipts.Add(receipt5);
            Context.Receipts.Add(receipt6);
            Context.Receipts.Add(receipt7);
            Context.Receipts.Add(receipt8);
        }

    }
}
