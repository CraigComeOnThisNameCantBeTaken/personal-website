Please Note that Octokit was considered for this project however there is no way to get a list
of projects a user has contributed on (only their owned projects) via the Octokit User Api,
and there is no way to get repositories a user a has comitted to bia the Repository Api.

For that reason, and because of the simplicity of the problem, simple HTTP requests have been chosen for
GitHub. These are:
https://api.github.com/users/CraigComeOnThisNameCantBeTaken
https://api.github.com/users/CraigComeOnThisNameCantBeTaken/repos
https://api.github.com/repos/CraigComeOnThisNameCantBeTaken/personal-website/commits

Please note the rate limit is 5000 vs 60 requests per hour based on authentication, however
this project will not need to make even close to 60 and so the authentication complexity has been skipped.

The on demand aspects of this project are intended to be used for creating a fresh persistance store, or
for resychronising. As such there should not be a need to use this project per-request.