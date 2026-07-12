// WebApplicationFactory's HostFactoryResolver relies on process-global state, so building
// multiple test hosts concurrently is unsafe. Run test collections sequentially.
[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]
