# DelauNET
Triangulation library that's not Triangle.NET .

# Features
It works; that's a pretty important part of a triangulation library.

I found that ports of other applications (most notably, [Triangle](http://www.cs.cmu.edu/~quake/triangle.html) 
and [Triangle.NET](https://archive.codeplex.com/?p=triangle)) are abandoned, do not compile out of the box or both. 
Whether that's a me-problem or not, I don't know, but I ended up writing this in a small bout of frustration over 
not getting other solutions to function.

It's not optimized.  
It does not handle edge cases well (Point lands on top of circumcircle; what now?).  
The types defined aren't special and just contain a bunch of helpers.  
