Qicktime ATOM Parser in .NET
============================

A direct port of Quicktime ATOM parser in java from http://www.onjava.com/pub/a/onjava/2003/02/19/qt_file_format.html with a few edits.
Example atom hirarchy output for a .mov file:
```
moov (8 KB) - 6 children
  mvhd (108 bytes)
  trak (2 KB) - 4 children
    tkhd (92 bytes)
    edts (36 bytes) - 1 child
      elst (28 bytes)
    mdia (2 KB) - 3 children
      mdhd (32 bytes)
      hdlr (58 bytes)
      minf (2 KB) - 4 children
        smhd (16 bytes)
        hdlr (57 bytes)
        dinf (36 bytes) - 1 child
          dref (28 bytes)
        stbl (2 KB) - 5 children
          stsd (187 bytes)
          stts (24 bytes)
          stsc (460 bytes)
          stsz (1 KB)
          stco (164 bytes)
    udta (12 bytes) - 0 children
  trak (2 KB) - 4 children
    tkhd (92 bytes)
    edts (36 bytes) - 1 child
      elst (28 bytes)
    mdia (1 KB) - 3 children
      mdhd (32 bytes)
      hdlr (58 bytes)
      minf (1 KB) - 4 children
        vmhd (20 bytes)
        hdlr (57 bytes)
        dinf (36 bytes) - 1 child
          dref (28 bytes)
        stbl (1 KB) - 6 children
          stsd (175 bytes)
          stts (24 bytes)
          stss (24 bytes)
          stsc (256 bytes)
          stsz (580 bytes)
          stco (168 bytes)
    udta (12 bytes) - 0 children
  trak (2 KB) - 5 children
    tkhd (92 bytes)
    edts (36 bytes) - 1 child
      elst (28 bytes)
    tref (20 bytes) - 1 child
      hint (12 bytes)
    mdia (1 KB) - 3 children
      mdhd (32 bytes)
      hdlr (51 bytes)
      minf (1 KB) - 4 children
        gmhd (32 bytes)
        hdlr (57 bytes)
        dinf (36 bytes) - 1 child
          dref (28 bytes)
        stbl (1 KB) - 6 children
          stsd (52 bytes)
          stts (32 bytes)
          stss (24 bytes)
          stsc (244 bytes)
          stsz (580 bytes)
          stco (164 bytes)
    udta (524 bytes) - 3 children
      name (26 bytes)
      hnti (240 bytes)
      hinf (246 bytes)
  trak (1 KB) - 5 children
    tkhd (92 bytes)
    edts (36 bytes) - 1 child
      elst (28 bytes)
    tref (20 bytes) - 1 child
      hint (12 bytes)
    mdia (856 bytes) - 3 children
      mdhd (32 bytes)
      hdlr (51 bytes)
      minf (765 bytes) - 4 children
        gmhd (32 bytes)
        hdlr (57 bytes)
        dinf (36 bytes) - 1 child
          dref (28 bytes)
        stbl (632 bytes) - 5 children
          stsd (52 bytes)
          stts (24 bytes)
          stsc (364 bytes)
          stsz (20 bytes)
          stco (164 bytes)
    udta (515 bytes) - 3 children
      name (26 bytes)
      hnti (223 bytes)
      hinf (254 bytes)
  udta (463 bytes) - 1 child
    hnti (451 bytes)
free (32 bytes)
wide (8 bytes)
mdat (414 KB)
```