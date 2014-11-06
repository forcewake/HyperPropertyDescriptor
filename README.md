HyperPropertyDescriptor
=======================

Provides a vastly accelerate runtime property implementation that can be applied even to closed-source classes

## Source

Based on the code from the [Marc Gravell article][1]

[1]: http://www.codeproject.com/Articles/18450/HyperDescriptor-Accelerated-dynamic-property-acces

## Results

### MyEntity.Name with 25000000 operations

| Operation            	| Direct access 	| Without HyperTypeDescriptionProvider 	| With HyperTypeDescriptionProvider 	|
|----------------------	|---------------	|--------------------------------------	|-----------------------------------	|
| GetProperties        	|               	| 647ms                                	| 699ms                             	|
| IsReadOnly           	|               	| 2926ms                               	| 43ms                              	|
| SupportsChangeEvents 	|               	| 245ms                                	| 41ms                              	|
| GetValue             	| 8ms           	| 10360ms                              	| 57ms                              	|
| SetValue             	| 97ms          	| 20288ms                              	| 155ms                             	|
| ValueChanged         	| 1022ms        	| 29566ms                              	| 954ms                             	|

### MySuperEntity.Name with 25000000 operations

| Operation            	| Without HyperTypeDescriptionProvider 	| With HyperTypeDescriptionProvider 	|
|----------------------	|--------------------------------------	|-----------------------------------	|
| GetProperties        	| 828ms                                	| 914ms                             	|
| IsReadOnly           	| 2881ms                               	| 41ms                              	|
| SupportsChangeEvents 	| 241ms                                	| 44ms                              	|
| GetValue             	| 10682ms                              	| 95ms                              	|
| SetValue             	| 20730ms                              	| 173ms                             	|
| ValueChanged         	| 30979ms                              	| 1059ms                            	|

### MySuperEntity.When with 10000000 operations

| Operation            	| Without HyperTypeDescriptionProvider 	| With HyperTypeDescriptionProvider 	|
|----------------------	|--------------------------------------	|-----------------------------------	|
| GetProperties        	| 825ms                                	| 891ms                             	|
| IsReadOnly           	| 2888ms                               	| 41ms                              	|
| SupportsChangeEvents 	| 251ms                                	| 46ms                              	|
| GetValue             	| 11393ms                              	| 295ms                             	|
| SetValue             	| 22416ms                              	| 110ms                             	|

## Example

Just write this piece of code:

```csharp

HyperTypeDescriptionProvider.Add(typeof (MyEntity));


```