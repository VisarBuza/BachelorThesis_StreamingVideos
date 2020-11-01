![](resources/hashcode2017\_qualification\_task.001.png)

Streaming   videos

Problem   statement   for   Online   Qualification   Round,   Hash   Code   2017


Introduction

Have you ever wondered what happens behind the scenes when you watch a YouTube video? As more and  more  people  watch  online  videos  (and  as  the  size  of  these  videos  increases),  it  is  critical  that video-serving   infrastructure   is   optimized   to   handle   requests   reliably   and   quickly. 

This typically involves putting in place cache servers, which store copies of popular videos. When a user request for a particular video arrives, it can be handled by a cache server close to the user, rather than by a remote   data   center   thousands   of   kilometers   away. 

But   how   should   you   decide   which   videos   to   put   in   which   cache   servers? 

##Task

Given a description of cache servers, network endpoints and videos, along with predicted requests for individual videos,  **decide which videos to put in which cache server** in order to minimize the average waiting   time   for   all   requests. 

Problem   description

The   picture   below   represents   the   video   serving   network. 

![](resources/hashcode2017\_qualification\_task.002.png)

**Videos**

Each video has a size given in megabytes (MB). The data center stores **all**  **videos** . Additionally, each video can  be  put  in  0,  1,  or  more  **cache  servers** .  Each  cache  server  has  a  maximum  capacity  given  in megabytes. 

**Endpoints**

Each **endpoint****   represents a group of users connecting to the Internet in the same geographical area (for example, a neighborhood in a city). Every endpoint is connected to the data center. Additionally, each endpoint   may   (but   doesn’t   have   to)   be   connected   to   1   or   more   cache   servers **.** 

Each endpoint is characterized by the latency of its connection to the data center (how long it takes to serve a video from the data center to a user in this endpoint), and by the latencies to each cache server that the endpoint is connected to (how long it takes to serve a video stored in the given cache server to a user in this   endpoint). 

**Requests**

The predicted  **requests** provide data on how many times a particular video is requested from a particular endpoint. 

Input   data   set

The input data is provided as a data set file - a plain text file containing exclusively ASCII characters with a single   ‘\n’   character   at   the   end   of   each   line   (UNIX- style   line   endings). 

Videos, endpoints and cache servers are referenced by integer IDs. There are  ***V*** videos numbered from 0 to   *V* − 1,   E   endpoints   numbered   from   0   to   *E* − 1   and   C   cache   servers   numbered   from   0   to   *C* − 1. 
