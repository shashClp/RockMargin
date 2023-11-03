# RockMargin release process
This document describes RockMargin release process.

1. Push latest changes to GitHub
   - Update `CHANGELOG.md` with release notes
   - Push changes to the _origin_
1. Publish new GitHub Release 
   - Initiate new deployment in [AppVeyor](https://ci.appveyor.com/project/K1tty/rockmargin/deployments/new)
   - Open generated [release draft](https://github.com/K1tty/RockMargin/releases)
   - Fill description with release notes from `CHANGELOG.md`
   - Publish
1. Publish to the [Marketplace](https://marketplace.visualstudio.com/items?itemName=K1tty.RockMargin)
   - Updload new _vsix_
   - Update _overview_
   - Publish
