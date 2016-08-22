# Functions:
square = (x) -> x * x

# Objects:
math =
  root:   Math.sqrt
  square: square
  cube:   (x) -> x * square x