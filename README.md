# TILRunTime-framework


[![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/ALEXTANGXIAO/TILRunTime-framework/master/LICENSE.TXT)
[![release](https://img.shields.io/badge/release-v1.0.4-blue.svg)](https://github.com/ALEXTANGXIAO/TILRunTime-framework/issues)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blue.svg)](https://github.com/ALEXTANGXIAO/TILRunTime-framework/pulls)

## TILRunTime-framework iOS、Android、Switch、PC等全平台热更方案

![Build status](http://106.52.118.65:1000/src/github.png)

<!-- ![Build status](http://106.52.118.65:1000/src/TILQR.png) -->

# ILRuntime实现原理

### ILRuntime借助Mono.Cecil库来读取DLL的PE信息，以及当中类型的所有信息，最终得到方法的IL汇编码，然后通过内置的IL解译执行虚拟机来执行DLL中的代码。IL解释器代码在ILIntepreter.cs，通过Opcode来逐语句执行机器码.
<br/>

# 需求说明

### 1. 部分系统例如：ios 封了内存的执行权限所以没法JIT热更新。
<br/>

### 2. 常规的CLR需要JIT编译方法为机器语言后存入内存，再调用该内存执行，所以需要内存执行权限
<br/>

### 3. ILRuntime实现了对于IL的解释执行，解释执行逐句解析il指令，同时维护一个stackframe用于模拟cpu的函数调用的基本操作进行辅助解释。
<br/>

### 4. 在框架层这边都是对应的同一个warper,即iltyeinstance。iltyeinstance会知道最终被调用方法的il指令内容，如果调用，则就是switch逐句去解析这个方法的il码，这里面会发现没有什么执行权限的问题，简单理解为读取一个普通文件，然后解析文件内容。如果是反射处理这种情形，那就是真实的构建出一个新的类型，然后调用新类型的方法，这倒是会涉及到内存权限问题。
<br/>

### 5. 整个过程中没有涉及到新类型的生成，都是iltypeinstance，没有涉及到新类型的内存执行权限的问题。
<br/>

# 托管调用栈
### ILRuntime在进行方法调用时，需要将方法的参数先压入托管栈，然后执行完毕后需要将栈还原，并把方法返回值压入栈。

### 具体过程如下图所示
```
调用前:                                调用完成后:
|---------------|                     |---------------|
|     参数1     |     |-------------->|   [返回值]    |
|---------------|     |               |---------------|
|      ...      |     |               |     NULL      |
|---------------|     |               |---------------|
|     参数N     |     |               |      ...      |
|---------------|     |
|   局部变量1    |     |
|---------------|     |
|      ...      |     |
|---------------|     |
|   局部变量1    |     |
|---------------|     |
|  方法栈基址    |     |
|---------------|     |
|   [返回值]     |------
|---------------| 
```
### 函数调用进入目标方法体后，栈指针（后面我们简称为ESP）会被指向方法栈基址那个位置，可以通过ESP-X获取到该方法的参数和方法内部申明的局部变量，在方法执行完毕后，如果有返回值，则把返回值写在方法栈基址位置即可（上图因为空间原因写在了基址后面）。