# Linqify
Toolkit to create Linq providers for asynchronous REST APIs

[Documentation in progess]

## Samples
If you want to see any sample usage of this toolkit, you can take a look at the following projects:

* [LinqToVSO](https://github.com/cjgaliana/LinqToVSO)
* [GianBombClient](https://github.com/cjgaliana/GiantBombClient)


## Usage
### Context
The context is the main entry point for the user. 
In order to create a new context, just create a new class that inherhits from "LinqifyContext".

You will need to use an implementation of ILinqifyExecutor in order to pass it to the base constructor

```cs
public class MyContext : LinqifyContext
{

    public VsoContext(ILinqifyExecutor executor)
     : base(executor)
    {
    }

...
```

You will need to override the CreateRequestProcessor<T>(string requestType) method. 
This method creates the required Request Processor depending on the requested entity type; 
For example:

```cs
protected override IRequestProcessor<T> CreateRequestProcessor<T>(string requestType)
{
    IRequestProcessor<T> req = return new TeamRequestProcessor<T>();
    req.BaseUrl = "API BASE URL"; // You can add more parameters using the "custom parameters" propertiy
    return req;
}
```

### Entities
The entities are the set of data the user can query. Entities are created using the LinqifyQueryable<T> class.

In your context class, just add all the 
entities you want to give to the user. For example, if you want to expose a list of Teams, just add the following property:

```cs
public LinqifyQueryable<Team> Teams
{
    get
    {
        return new LinqifyQueryable<Team>(this); // Where "this" is the context
    }
}
```


### Executor
This class makes the actual HTTP petition. In the implementation of this class, you can add all the logic your API needs, like custom headers, authentication, etc...


### Processors
The main class of your app. This interface contaisn 3 main methods you have to implement:

**GetParameters**

Extracts the parameters from the Lambda expresion

For example, this would be one example for the Visual Studio Oonline API:

```cs
public virtual Dictionary<string, string> GetParameters(LambdaExpression lambdaExpression)
{
    return
        new ParameterFinder<Team>(
            lambdaExpression.Body,
            new List<string>
            {
                TakeClauseFinder.TakeMethodName, //Number of team projects to return.
                SkipClauseFinder.SkipMethodName, //Number of team projects to skip
                "Id", //If this parameter exists, gets the info for the given ID
                "ProjectId" //The parent project
            })
            .Parameters;
}
```


**BuildUrl**

Given the parameters sent using the Linq query, creates the final URL to get the response.



For example, this would be one example for the Visual Studio Oonline API:

```cs
public virtual Request BuildUrl(Dictionary<string, string> expressionParameters)
{
    if (!expressionParameters.ContainsKey("ProjectId"))
    {
        throw new ArgumentException("A project ID is required to perform this operation");
    }

    if (expressionParameters.ContainsKey("Id"))
    {
        return this.GetTeamDetailsUrl(expressionParameters);
    }

    this._projectId = expressionParameters["ProjectId"];

    var url = string.Format("{0}/{1}/{2}/{3}",
        this.BaseUrl,
        "_apis/projects",
        this._projectId,
        "teams");
    var req = new Request(url);
    var urlParams = req.RequestParameters;

    if (expressionParameters.ContainsKey(TakeClauseFinder.TakeMethodName))
    {
        urlParams.Add(new QueryParameter("$top", expressionParameters[TakeClauseFinder.TakeMethodName]));
    }

    if (expressionParameters.ContainsKey(SkipClauseFinder.SkipMethodName))
    {
        urlParams.Add(new QueryParameter("$skip", expressionParameters[SkipClauseFinder.SkipMethodName]));
    }

    urlParams.Add(new QueryParameter("api-version", "1.0"));
    return req;
}
```


**ProcessResult**

Given the API response, parses the result in order to return the final object(s)

For example, this would be one example for the Visual Studio Oonline API:

```cs
public List<T> ProcessResults(string vsoResponse)
{
    var json = JObject.Parse(vsoResponse);
    var serverData = json["value"].Children().ToList();

    var resultList = new List<Team>();

    foreach (var data in serverData)
    {
        var item = JsonConvert.DeserializeObject<Team>(data.ToString());
        resultList.Add(item);
    }

    return resultList.OfType<T>().ToList();
}
```




