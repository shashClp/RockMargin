# RockMargin

In VS 2005/2008 days I've found the tool that makes code navigation easy and fast. It was named `RockScroll`. Year by year I used this tool every day and become addicted to it. But then came era of VS2010 and my world crashed into pieces because RockScroll was never ported to new versions of Visual Studio. I tried loads of possible replacemens but haven't found any that could make me as happy as I was in the days of RockScroll. So as usually happens with programmers I decided to write my own.

##### Vertical scrollbar

This extension replaces Visual Studio vertical scrollbar with current text document overview that works as scrollbar at the same time. And there is the magic happens: on source files with up to few thousands of lines its really easy to navigate through your code using thumbnail view of the file.

![vertical scrollbar](./RockMargin/Info/vertical_scrollbar.png)

##### Words highlighting

Double clicking on any word in the text highlight it both in text editor and in scrollbar. Using this feature you can easely track all occurences of some word in your file without using text search.

Right clicking on scrollbar removes highlighting.

![words highlighting](./RockMargin/Info/words_highlighting.png)

##### Boormarks & Breakpoints

Also you can see all breakpoints and bookmarks in current source file - they are displayed on the left side of scrollbar.

![breakpoints](./RockMargin/Info/breakpoints.png)

##### Comments highlighting

I tried to make full syntax highlighting in thumbnail view of text but it appeared to be quite complex thing to do both algorithmically and performance wise. Also with some early prototypes of syntax highlighting I've found that on some files scrollbar becomes as colorful as Rio carnaval at night which is not very helpfull when you writing your code. So I decided to leave only comments coloring like RockScroll had. Both single line and miltiline comments supported.

At the moment comments coloring supported only for the next languages:
- C/C++
- C#
- VisualBasic
- LUA
- XML

Choice of languages is quite presonal - its set of languages I usually use. If some of you knows language-agnostic way to track comments without spending too much CPU cycles I would be glad to read your proposals and implement language-independent comments traking.

##### Outlining support

Its a Visual Studio feature that RockScroll never supported and this fact distracted many programmers from using it. RockMargin supports it natively.

##### Split windows support

Another one feature RockScroll had problems with. Its supported out of the box too.

##### Customizable options

To make RockMargin play nicely with all possible Visual Studio color themes all colors are fully customizable. Also you can disable words highlighting feature if you prefer similar one from some other extension you use (like [VisualAssist]) or Visual Studio built-in.

[VisualAssist]: http://www.wholetomato.com

![options](./RockMargin/Info/options.png)

##### Performance/memory optimized

One of my primary goals was consuming as low resources as possible to not affect your Visual Studio experience in any way.

As result this extension works almost instantly on nowadays PC's with files up to 10000 lines of code. With million lines of code you can experience lowered redraw speeds (about 2-3 seconds) but Visual Studio performance will not be affected by this extension even with such huge files because actual rendering and file parsing done in worker thread.

##### Changelog

* 1.3.1 - Oct 13, 2013
  * Fixed saving/loading of Alt + Double Click option
* 1.3.0 - Sep 22, 2013
  * Enhanced text rendering option
* 1.2.0 - Sep 19, 2013
  * Enabled VS2013 support
* 1.2.0 - Sep 17, 2013
  * Added change margin
* 1.1.2 - Aug 03, 2013
  * Fixed VS2010 crash
* 1.1.1 - Aug 29, 2013
  * Fixed bug in multiline comments tracking when entire scrollbar text could be rendered as comment
* 1.0.12 - Aug 29, 2013
  * Added option for words highlighting with Alt + Double Click
* 1.0.11 - Aug 28, 2013
  * Clearing words highlights by Esc button
  * Disable words highlighting in non-document text views
* 1.0.10 - Aug 03, 2013
  * basic extension stability monitoring
* 1.0.9 - Jul 19, 2013
  * Fixed VS2010 crash when closing cpp tab with active breakpoint
* 1.0.8 - Mar 06, 2013
  * Added VS2010 support
* 1.0.7 - Nov 05, 2012
  * Fix: proper update of breakpoints & bookmarks positions with code outlining enabled
* 1.0.6 - Oct 31, 2012
  * Fix: don't break ReSharper margin anymore
* 1.0.5 - Oct 25, 2012
  * added margin width option
* 1.0.4 - Oct 21, 2012
  * Fix: proper margin rendering with non-default background color
* 1.0.3 - Oct 20, 2012
  * Fixed options persistence
  * Antialiased text preview rendering with large text files
* 1.0.2 - Oct 17, 2012
  * Fixed crash on attempt to highligh words in html
* 1.0.1 - Oct 15, 2012
  * Fixed crash when opening files with user defined regions (#region/#endregion)
* 1.0.0 - Oct 14, 2012
  * initial release

___

##### Works On My Machine Disclaimer
> This is released with exactly zero warranty or support. If it deletes files or kills your family pet, you have been warned. It might work great, and it might not. It hasn't been tested against the myriad of other VS Add-Ins, but it works well on my machine.

<div style="text-align:center"><img src ="./RockMargin/Info/works_on_my_machine.png" /></div>

___

**P.S:** I’d like to say thanks to [Scott Hanselman] who released RockScroll - great tool that completely changed the way I’m writing code in Visual Studio.

[Scott Hanselman]: http://www.hanselman.com/blog/