# Slidable CLI

This repo contains the Slidable command-line tool, which provides a simple, cross-platform,
browser-based slide presentation app that also interacts with an online service, by default
at [slidable.io](https://slidable.io) (although you can host your own), allowing people watching
your talk to ask questions without interrupting, and to take notes against each slide that they
can refer back to later.

# Usage

Some changes are being made to the Markdown document format that Slidable uses; once that is
stable documentation will be made available on the main Slidable website.

# Tech

A key goal for the CLI tool is to distribute it as a self-contained, native executable*. To that
end, it is designed to be compilable with [.NET CoreRT](https://github.com/dotnet/corert), a
runtime and suite of tools for doing Ahead-of-Time (AoT) compilation of .NET Core applications.

To that end, this is an ASP<span></span>.NET Core application that doesn't use Controllers or
Razor, because those don't work well with AoT compilation. Instead it uses the lower-level Routing
API, and handles everything manually. It also embeds a bunch of files in the binary using the
decidedly non-standard approach of creating a massive array of bytes in a static class, because
the actual resource embedding wasn't working on Linux when I wrote this.

This is also why this project is still on ASP<span></span>.NET Core 2.0; I've been having problems
getting the 2.1 bits to compile with CoreRT, although I'm confident that this will be resolved
soon.

## *Why the native executable thing?

OK, so this project came out of my experiences doing talks from a laptop running Linux. The best
tools for doing that are browser-based ones like Reveal.js, which work great for developers, but
for non-developers the experience of using these "light-weight" tools is probably daunting.

```
PRESENTER: So, how do I get this Reveal.js thing then?

DEVELOPER: Right, so first of all you need to install Node.js.

P: What's Node.js?

D: It's a JavaScript runtime for building web applications.

P: Not a presentation app?

D: No, but you can get presentation apps written for Node.js.

P: Hm. OK. Then what do I do?

D: Well, next you have to clone the Reveal.js repository from GitHub and...

P: Stop.

D: ...run NPM install...

P: Please stop.

D: ...and wait while your laptop downloads the internet...

P: [EXIT, PURSUED BY A WOMBAT]
```

You get the idea.

So it's very important to me that when you want to get Slidable, you can download a ZIP file
that contains a single, self-contained binary executable, called `slidable.exe` on Windows and
just `slidable` on Mac and Linux, and put it on your path somewhere, and then that's it.
Because that's how we win.

# Contributing

If you'd like to contribute to Slidable CLI's development, there's plenty of stuff to do. One
thing I'd really like to see is more themes, especially ones designed by people with more
artistic talent than [a cluster of colour-blind hedgehogs... in a bag](http://blackadderquotes.com/artistic-talent). But also just general improvements; if you have an
idea to make the thing more usable or better, please [create an Issue](https://github.com/slidable/cli/issues) to discuss it and then fork away. Check out some of the
awesome stuff that Reveal.js does if you want some ideas.
