(() => {
    const form = document.querySelector('form[data-wysiwyg-form]');
    if (!form) {
        return;
    }

    const toolbarLayout = [
        { command: 'bold', label: 'Bold', toggle: true },
        { command: 'italic', label: 'Italic', toggle: true },
        { command: 'underline', label: 'Underline', toggle: true },
        { command: 'insertUnorderedList', label: 'Bullets' },
        { command: 'insertOrderedList', label: 'Numbers' },
        { command: 'createLink', label: 'Link', action: 'link' },
        { command: 'removeFormat', label: 'Clear', action: 'clear' }
    ];

    const editors = [];
    const EMPTY_PARAGRAPH = '<p></p>';

    const escapeHtml = (value) => value
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;');

    const normalizeInitialValue = (value) => {
        if (!value) {
            return EMPTY_PARAGRAPH;
        }

        const trimmed = value.trim();
        if (trimmed.length === 0) {
            return EMPTY_PARAGRAPH;
        }

        const containsTags = /<\/?[a-z][\s\S]*>/i.test(trimmed);
        if (containsTags) {
            return trimmed;
        }

        const html = trimmed
            .split(/\r?\n/)
            .map(line => line.trim())
            .filter(Boolean)
            .map(line => `<p>${escapeHtml(line)}</p>`)
            .join('');

        return html.length > 0 ? html : EMPTY_PARAGRAPH;
    };

    const updateToolbarState = (editorEntry) => {
        if (!editorEntry) return;
        const selection = window.getSelection();
        if (!selection || selection.rangeCount === 0) return;
        const range = selection.getRangeAt(0);
        if (!editorEntry.editor.contains(range.startContainer)) {
            editorEntry.buttons.forEach(btn => btn.element.classList.remove('active'));
            return;
        }

        editorEntry.buttons.forEach(btn => {
            if (!btn.toggle) return;
            try {
                const state = document.queryCommandState(btn.command);
                btn.element.classList.toggle('active', Boolean(state));
            } catch (err) {
                btn.element.classList.remove('active');
            }
        });
    };

    const applyCommand = (entry, buttonConfig) => {
        if (!buttonConfig) return;
        entry.editor.focus();

        if (buttonConfig.action === 'link') {
            const url = window.prompt('Paste the URL for this link:');
            if (url) {
                document.execCommand('createLink', false, url.trim());
            }
            return;
        }

        if (buttonConfig.action === 'clear') {
            document.execCommand('removeFormat', false);
            document.execCommand('unlink', false);
            return;
        }

        document.execCommand(buttonConfig.command, false, buttonConfig.value ?? null);
    };

    const createToolbar = (entry) => {
        const toolbar = document.createElement('div');
        toolbar.className = 'wysiwyg-toolbar';

        toolbarLayout.forEach(config => {
            const button = document.createElement('button');
            button.type = 'button';
            button.textContent = config.label;
            button.dataset.command = config.command;
            button.addEventListener('mousedown', (event) => {
                event.preventDefault();
            });
            button.addEventListener('click', (event) => {
                event.preventDefault();
                applyCommand(entry, config);
                updateToolbarState(entry);
            });
            toolbar.appendChild(button);
            entry.buttons.push({ element: button, command: config.command, toggle: Boolean(config.toggle), action: config.action });
        });

        return toolbar;
    };

    const initialiseEditor = (textarea) => {
        const wrapper = document.createElement('div');
        wrapper.className = 'wysiwyg-wrapper mt-2';

        const editor = document.createElement('div');
        editor.className = 'wysiwyg-editor';
        editor.contentEditable = 'true';
        editor.innerHTML = normalizeInitialValue(textarea.value);

        const hint = document.createElement('span');
        hint.className = 'wysiwyg-hint';
        hint.textContent = 'Use the toolbar to format content. You can insert placeholders like {FullName} or {TourName}.';

        const entry = { textarea, editor, buttons: [] };
        const toolbar = createToolbar(entry);

        textarea.classList.add('d-none');
        const parent = textarea.parentElement;
        parent.insertBefore(wrapper, textarea);
        wrapper.appendChild(toolbar);
        wrapper.appendChild(editor);
        parent.appendChild(hint);
        wrapper.appendChild(textarea);

        editor.addEventListener('input', () => {
            updateToolbarState(entry);
        });

        editor.addEventListener('focus', () => {
            updateToolbarState(entry);
        });

        editor.addEventListener('paste', (event) => {
            event.preventDefault();
            const text = (event.clipboardData || window.clipboardData).getData('text/plain');
            if (!text) return;
            const html = escapeHtml(text).replace(/\r?\n/g, '<br/>');
            document.execCommand('insertHTML', false, html);
        });

        editors.push(entry);
    };

    form.querySelectorAll('textarea[data-wysiwyg="true"]').forEach(initialiseEditor);

    document.addEventListener('selectionchange', () => {
        const active = editors.find(entry => {
            const selection = window.getSelection();
            if (!selection || selection.rangeCount === 0) return false;
            const range = selection.getRangeAt(0);
            return entry.editor.contains(range.startContainer);
        });
        updateToolbarState(active);
    });

    form.addEventListener('submit', () => {
        editors.forEach(entry => {
            const value = entry.editor.innerHTML
                .replace(/<p>\s*<\/p>/g, '')
                .trim();
            entry.textarea.value = value.length === 0 ? EMPTY_PARAGRAPH : value;
        });
    });
})();
