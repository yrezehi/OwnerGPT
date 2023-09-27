namespace OwnerGPT.LLM.Native
{
    using LlamaToken = Int32;

    internal class LlamaCppSessionState
    {
        public List<LlamaToken> TokenIds { get; } = new();
        public int EvalOffset { get; set; } = 0;
    }

    public class NativeLLamaSession
    {
        private NativeLLamaModel _model;
        private LlamaCppSessionState _state = new();
        private Guid _id = Guid.NewGuid();

        private static NativeLLamaSession? _lastSessionToGenerate;

        internal NativeLLamaSession(NativeLLamaModel model) => _model = model;

        public Guid Id => _id;

        public void Reset()
        {
            _state.TokenIds.Clear();
            _state.EvalOffset = 0;
        }

        public string GetContextAsText() => _model.UntokenizeToText(_state.TokenIds);

        public IAsyncEnumerable<byte[]> GenerateTokenBytesAsync(string prompt, NativeLlamaGenerateOptions? options = default, CancellationToken cancellationToken = default)
        {
            if (_lastSessionToGenerate != null && _lastSessionToGenerate != this)
                _state.EvalOffset = 0;

            _lastSessionToGenerate = this;

            if (options == default)
                options = new();

            _state.TokenIds.AddRange(_model.Tokenize(prompt, !_state.TokenIds.Any()));
            return _model.GenerateTokenBytesAsync(options, _state, cancellationToken);
        }

        public IAsyncEnumerable<byte[]> GenerateTokenBytesAsync(string prompt, CancellationToken cancellationToken = default) =>
            GenerateTokenBytesAsync(prompt, default, cancellationToken);

        public IAsyncEnumerable<string> GenerateTokenStringAsync(string prompt, NativeLlamaGenerateOptions? options = default, CancellationToken cancellationToken = default)
        {
            if (_lastSessionToGenerate != null && _lastSessionToGenerate != this)
                _state.EvalOffset = 0;

            _lastSessionToGenerate = this;

            if (options == default)
                options = new();

            var tokens = _model.Tokenize(prompt, !_state.TokenIds.Any());
            _state.TokenIds.AddRange(tokens);

            return _model.GenerateTokenStringAsync(options, _state, cancellationToken);
        }

        public IAsyncEnumerable<string> GenerateTokenStringAsync(string prompt, CancellationToken cancellationToken = default) =>
            GenerateTokenStringAsync(prompt, default, cancellationToken);

        public IAsyncEnumerable<string> GenerateStatelessTokenStringAsync(string prompt, NativeLlamaGenerateOptions? options = default, CancellationToken cancellationToken = default)
        {
            return _model.GenerateStatlessTokenStringAsync(options, _model.Tokenize(prompt, !_state.TokenIds.Any()), cancellationToken);
        }

    }
}
