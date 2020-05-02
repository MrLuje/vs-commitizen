# vs-commitizen

[![Build status](https://ci.appveyor.com/api/projects/status/4yx0hjn5qmu8oem0/branch/master?svg=true)](https://ci.appveyor.com/project/MrLuje/vs-commitizen/branch/master)

This extension adds [commitizen](https://github.com/commitizen/) support to VisualStudio.

## Features

- Add link in _Source control_ menu and _Home_ view

![vs-commitizen_-_home__1.png](images/home.png)

- Add button near to the "Commit" button in _Changes_ view to easily use commitizen

![vs-commitizen_-_commit-cz.png](images/commit-cz.png)

- Nice page to format your comment using commitizen fashion.

![vs-commitizen_-_commitizen_view.png](images/commitizen-view.png)

## Customizations

The list of "Type of changes" can be customized, globally or per repository.

The configuration is stored in a *.commitizen.json* file ([schema](./config-schema.json))

You can access the configuration file directly from VisualStudio menu (files will be generated if not existing yet) :


![menu.png](images/menu.png)

#### Sample configuration

```json
{
  "$schema": "https://github.com/MrLuje/vs-commitizen/config-schema.json",
  "types": [
    {
      "type": "feat",
      "description": "A new feature"
    },
    {
      "type": "fix",
      "description": "A bug fix"
    },
    {
      "type": "docs",
      "description": "Documentation only changes"
    },
    {
      "type": "test",
      "description": "Adding missing tests or correcting existing tests"
    }
}
```