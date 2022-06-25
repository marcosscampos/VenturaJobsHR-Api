using System;
using System.Collections.Generic;

namespace VenturaJobsHR.Application.Records.Applications;

public record ApplicationResponse(UserRecord User, string JobId, List<JobApplicationCriteriaRecord> CriteriaList, double Average);