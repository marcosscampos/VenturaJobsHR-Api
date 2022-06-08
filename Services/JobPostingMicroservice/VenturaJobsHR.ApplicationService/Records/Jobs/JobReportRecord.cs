using System.Collections.Generic;

namespace VenturaJobsHR.Application.Records.Jobs;

public record JobReportRecord(double JobAverage, List<UserValueRecord> UserValueList);

public record UserValueRecord(string Name, double Average);