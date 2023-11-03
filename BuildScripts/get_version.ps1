Write-Host "Calculating RockMargin version... " -ForegroundColor Cyan -NoNewline

# find last commit date
$last_commit = git log -1 --format=%cd --date=format:'%Y,%m,%d'
$last_commit_date = [datetime]$last_commit

# calculate month start/end
$month_start = $last_commit_date.AddDays(1 - $last_commit_date.Day)
$month_end = $month_start.AddMonths(1).AddSeconds(-1)

# calculate commits during month
$commits = git log --format=oneline --since=$month_start --until=$month_end
$commits_count = $commits | Measure-Object -Line

# output version
$version = "{0}.{1}.{2}" -f $last_commit_date.Year, $last_commit_date.Month, $commits_count.Lines

$version | Write-Host -ForegroundColor Green

$version