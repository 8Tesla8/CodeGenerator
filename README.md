# CodeGenerator
Project allows to create code

read settings from filename.txt  

## commands
// - skip line


### class name
"name" : "MyClass"


### namespace
"namespace" : "MyClass.Model"


### using
"using" : "
System 
System.IO "
using System;
using System.IO;

"using" : "System System.IO"
using System;
using System.IO;


### inherit
"inherit": "Model, BaseModel"
MyClass : Model, BaseModel


### constructors
"ctor" : ""                          
public MyClass()

"ctor" : "private(int I, double d)"   
private MyClass(int I, double d)

"ctor" : "int I, double d"   
public MyClass(int I, double d)


### base constructor
"base-ctor": ""
: base()

"base-ctor": "int i, double d"
: base(int i, double d)


## files


### for model.txt 
generate ClassName.cs and ClassNameDTO.cs

name of properties
"prop":	" vehicle_id vehicle_number vin_number "

"prop":	"
vehicle_id
vehicle_number
vin_number "

result:
public VehicleId { get; set; }
public VehicleNumber { get; set; }
public VinNumber { get; set; }


### for repository.txt
generate Repository.cs
name of methods 
/ - is method separator 
; - sepearator signature and body 

example
"method" : " 
method1 signature; method1 body/
method2 signature; method2 body/"


"method":"
int GetAll(); <LayerIcon>(GetCategoryIcons)/
void Put(int i); <LayerIcon>(GetIcons)/" 

"method":"
int GetAll(); <Icon>(GetCategoryIcons)/
void Put(int i); <Icon>(GetIcons)/

result: 
public int GetAll()
  <Icon>(GetCategoryIcons)

public void Put(int i)
  <Icon>(GetIcons)


## example of settings in file
{
"name" : "MyClass",
"namespace" : "MyClass.Model",
"using" : "System System.IO",
"ctor" : "private(int I, double d)",
"base-ctor" : "",
"prop" :	"
vehicle_id
vehicle_number  // - will not generate this property because I wrote //
vin_number "
}
