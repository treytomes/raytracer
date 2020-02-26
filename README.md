# raytracer
My implementation of the Ray Tracer Challenge.

* [Book errata](https://pragprog.com/titles/jbtracer/errata)
* [Book forum](http://forum.raytracerchallenge.com/)

## Tuesday, February 18, 2020
I've started reading "The Ray Tracer Challenge" today.
The topic of ray tracing combines math and computer programming in one of my favorite ways.
This book claims to tackle it's subject using test-driven development,
which from my experience is incredibly difficult to do in any project that involves generating images,
so I'm eager to learn what he has to teach.

The book is designed for it's reader to translate pseudo-code and unit tests into any language and framework of choice.
I prefer .NET, and will probably pick on C# / .NET Core as that's the direction the Microsoft world is headed.
The unit tests are described using Cucumber syntax.
While I have tended to prefer xUnit for unit testing,
I will probably take this opportunity to learn the [SpecFlow library](https://specflow.org/).

## Wednesday, February 19, 2020
I decided to use xUnit after all.  It's just easier, and doesn't require addition Visual Studio extensions to play nicely.
For the time being I'm going to use WPF to do the rendering.  As the code doesn't require any rendering yet, this isn't a big
deal.  I'm not expecting to need hardware accelerated graphics for this, at least at first, so I'll simply be plotting
points onto an Image control.  It's not as bad as it sounds.

## Thursday, February 20, 2020
Still feeling conflicted on my Tuple code.  It's the basis of everything else, so it *must* be as perfect as possible.  Chapter 2 is accomplished.  I have made a point of doing the extra side-projects suggested by the author, so the Visual Studio solution also comes with an odd little projectile cannon simulator.  Next up is matrices.

## Wednesday, February 26, 2020
I got ray/sphere intersections in place, and my renderer can distort and draw a sphere.
Everything ran a lot faster when I started pre-calculating the inverse of the transformation matrix as soon as the transformation is initially calculated.
Suprisingly, turning the for-loops into Parallel.For loops didn't speed things up in a noticeable way.  I'll try it again later when the math becomes more complex.
