using System;

namespace AssignmentTeamLongestPeriod
{
    public class OverlapCase
    {
        public int Employee1ID { get; private set; }
        public int Employee2ID { get; private set; }
        public int ProjectID { get; private set; }
        public int DaysOn { get; private set; }

        public OverlapCase(DocumentRecord e1, DocumentRecord e2)
        {
            this.Employee1ID = e1.EmpID;
            this.Employee2ID = e2.EmpID;
            this.ProjectID = e1.ProjectID;
            this.DaysOn = (e1.DateFrom < e2.DateTo) ? Convert.ToInt32((e2.DateTo - e1.DateFrom).TotalDays) : Convert.ToInt32((e1.DateTo - e2.DateFrom).TotalDays);
        }
    }
}