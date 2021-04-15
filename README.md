## Design approach
I wanted to provide as simple API as it could be, but at the same time give control on every step of algorithm.
Thus, we can recognize differences between two images with just two lines of code

```csharp
using var calculator = ImgDiffCalculator.Create(new ImgDiffOptions("left.jpg", "right.jpg"));
using var diffImg = calculator.Calculate(25);
...
```

At first glance we don't have any control over process, but this is not true.
We can track progress of execution, provide custom algorithms and provide custom highlighters by implementing and providing corresponding interfaces.

## Strong and weak features
Strong features:
*  Fully customizable process of calculation
*  Well memory and speed optimized code
*  Well documented code
*  Almost garbage-less
*  Can compare highly noised images

Weak features:
* Necessity to tune algorithm error tolerance value for different images
* From my lack of knowledge in such algorithms it may be overcomplicated/nonoptimal
* Absence of parallelism and vectorization

## Ways to improve
* Get rid of `System.Drawing.Image` as it lacks of async versions of API and generates a lot of garbage
* Add vectorization and parallelism