# Config File Library

This is a hand-written text parser for TXT, YAML, and JSON files. It's designed for use in constructors
and NOT as efficient runtime collections. I simply use internal lists and dictionaries to store the data.

My primary use case is for something like a game, where a file would list all the properties of an enemy 
or item, and you would read from this object to get those parameters. My intention is to make it 
extremely explicit in the way items are read, so I allow for any amount of nested objects and arrays. 
Values are read out as strings, and you can convert them to any type that C# allows them to. This means 
arrays can have any type within them (string, int, double, and bool), and you can decide what they are 
at runtime. I've included a number of error messages to help you debug your files and ensure proper 
type matching back to C# primitives. 

See the wiki [here](https://wiki.codyhowell.dev/configfilelibrary) for more information.

## Weak Typing

This was accomplished by my primitive classes implementing an interface (IBaseConfigOption) that includes 
all of their methods and indexes. There are three implementing types, which I will describe below. 

The intended use of these is to have a large object or array, drill down into them via indexers 
(string or int respectively), and get a PrimitiveConfigOption at the end which you would read your value as. 
I can't really prevent you from assigning an Array or Object to something (and indeed, I think that would limit 
how clean the code for deeply nested objects would be), but my intent is to end every call with the full path 
and a method to make explicit what value is being read. 

### Primitive Config

This is a string, and has methods to convert it to any of the above listed types. All other methods throw 
an InvalidOperationExceptions. If a string can't be converted, it will throw a InvalidCastException.

### Array Config

This is a list of IBaseConfigOption objects (to allow for an assorted array of objects, arrays, and primitives). 
It has an int indexer to retreive specific elements, and four additional methods if everything in the array 
is of the same type (since you might be able to do more with a list of primitives). Otherwise, you can continue 
to index down into a primitive and get your value. 

Using the string indexing or attempting to read it as a primitive will throw an InvalidOperationException.

### Object Config

This is a Dictionary<string, IBaseConfigOption>. Its only valid property is the string indexer, which will
retrieve the IBaseConfigOption at that key. Everything else will return an InvalidOperationException.

## Usage

For all file types, you can read through my test cases to see what the file looks like and my test 
to see what exactly the call will look like. I've included my "Realistic" test cases for each file type
and their calls to read the values. In each case they start with "reader[...]", which is the ConfigFile object 
for each file type. The constructor just takes in the path to the file, and some have options for a different 
splitter or something like that. Read the XML comments for more information.

### Collector

I added a new class called `ConfigFileCollector` that takes in a list of filepaths and parses them 
all together. When you need a specific file, request it from the proper method and use it as normal. 

There's a number of exceptions thrown to help with the process. 

### TXT

This only supports single-order objects, though you can have arrays within them (which are split via commas). It's 
designed to be the simplest to use. Arrays can be inline or multiline, and you can have any amount of whitespace
around them. It reads to the next line that contains the closing "]", then splits the entire string via commas 
and assigns them each to a PrimitiveConfigOption. You can see those options in my example.

As this is the first, note that I don't use any double quotes anywhere; everything is split via the character
and then strings are trimmed afterwards, for both keys and values.

```txt
info: [John Doe, 29, 6.1, True]
data: [
    True,
    42,
    Seattle, ]
preferences: [ Travel,
    Music,
    3.14
]
address: 123 Main St
zip: 98101
```

```csharp
reader["info"][0].AsString(); // "John Doe"
reader["info"][3].AsBool(); // true
reader["data"][1].AsInt(); // 42
reader["address"].AsString(); // "123 Main St"

// Example of exporting a list
reader["info"].AsStringList(); // ["John Doe", "29", "6.1", "True"]
```

### YAML

This supports any amount of nested objects and arrays, and (somewhat) follows the YAML spec. I spent some time 
reading through it and as I am writing all the parsing logic, I've decided to stop where I'm at. 

Tabs are supported, and I also currently support only 4 space indents. The below object will show what I 
expect in terms of arrays of objects, and I think everything else is somewhat self-explanatory. If you'd 
like to submit a pull request for a better parser, I'd be happy to look at it. 

```yaml
first: 
    simple Array: 
        - 1
        - 2
        - 3
    brother: sample String
    other sibling: 
        sibKey: sibValue
        sibKey2: sibValue2
second: 
    arrayOfObjects: 
        - lorem: ipsum
          something: 1.2
        - lorem2: ipsum2
          something2: false
    otherThing: hopefully
```

```csharp
reader["first"]["simple Array"][1].AsInt(); // 2
reader["first"]["other sibling"]["sibKey"].AsString(); // "sibValue"
reader["second"]["arrayOfObjects"][0]["lorem"].AsString(); // "ipsum"
reader["second"]["arrayOfObjects"][0]["something"].AsDouble(); // 1.2
reader["second"]["arrayOfObjects"][1]["something2"].AsBool(); // false
```

### JSON

The way I'm implementing JSON requires some syntax specifics; first, you can only start with a 
[] or a {} for an array or object. Arrays are always split by commas ',' and objects are 
always keyed with a colon ':' and again split via commas. I don't support escape characters 
at the moment (or comments, in any form), but otherwise is formatted exactly like JSON.

This is the exact same data as stored in the above YAML file, referenced in the same way in C#.

```json
{
  "first": {
    "simple Array": [
      1,
      2,
      3
    ],
    "brother": "sample String",
    "other sibling": {
      "sibKey": "sibValue"
    }
  },
  "second": {
    "arrayOfObjects": [
      {
        "lorem": "ipsum",
        "something": 1.2
      },
      {
        "lorem2": "ipsum2",
        "something2": false
      }
    ],
    "otherThing": "hopefully"
  }
}
```

```csharp
reader["first"]["simple Array"][1].AsInt(); // 2
reader["first"]["other sibling"]["sibKey"].AsString(); // "sibValue"
reader["second"]["arrayOfObjects"][0]["lorem"].AsString(); // "ipsum"
reader["second"]["arrayOfObjects"][0]["something"].AsDouble(); // 1.2
reader["second"]["arrayOfObjects"][1]["something2"].AsBool(); // false
```


## Changelog

1.0 (6/25/25)

- Added new class to compile multiple file readers together, ConfigFileCollector
    - with a number of tests in many configurations
- Removed all warnings
- Updated all documentation

0.8 (5/12/25)

- Added JSON option

0.7 (3/8/25)

- Added a helper class for YAML parsing
- YAML parsing works as expected and passes all tests

0.4 (2/27/25)
	
- Primitive objects are fully implemented with errors and tests
- Arrays and objects seem to be working fine
- TXT files are working within spec and have tests
- Starting on YAML; built a helper class and hope to get it up soon
