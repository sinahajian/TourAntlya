(() => {
    const log = (...args) => console.log('[tour-booking]', ...args);

    const parseInput = (input, fallback = 0) => {
        if (!input) {
            return fallback;
        }

        const numeric = Number(input.value);
        if (!Number.isFinite(numeric)) {
            return fallback;
        }

        return Math.max(0, Math.floor(numeric));
    };

    const init = () => {
        const form = document.querySelector('.booking-form');
        if (!form) {
            log('booking form not found');
            return;
        }

        const steps = Array.from(form.querySelectorAll('.booking-step'));
        const progressSteps = Array.from(form.querySelectorAll('.booking-progress-step'));
        const nextButtons = Array.from(form.querySelectorAll('[data-next-step]'));
        const prevButtons = Array.from(form.querySelectorAll('[data-prev-step]'));

        const totalValue = document.getElementById('reservation-total-value');
        const totalHint = document.getElementById('reservation-total-hint');
        const summaryGuests = document.getElementById('summary-total-guests');
        const summaryPrice = document.getElementById('summary-total-price');

        const adultInput = form.querySelector('.js-booking-adults');
        const childInput = form.querySelector('.js-booking-children');
        const infantInput = form.querySelector('.js-booking-infants');
        const guestInputs = Array.from(form.querySelectorAll('.js-booking-input'));

        const payPalButton = document.getElementById('paypalCheckoutButton');
        const payPalContainer = document.getElementById('paypalCheckoutContainer');
        const paymentRadios = Array.from(form.querySelectorAll('input[name="Form.PaymentMethod"]'));

        const adultPrice = Number(form.dataset.adultPrice || 0);
        const childPrice = Number(form.dataset.childPrice || 0);
        const infantPrice = Number(form.dataset.infantPrice || 0);
        const payPalEnabled = form.dataset.paypalEnabled === 'true';
        const payPalBaseUrl = form.dataset.paypalBaseUrl || '';
        const payPalEmail = form.dataset.paypalEmail || '';
        const payPalCurrency = form.dataset.paypalCurrency || 'EUR';
        const payPalReturn = form.dataset.paypalReturn || '';
        const payPalCancel = form.dataset.paypalCancel || '';
        const tourName = form.dataset.tourName || 'Tour Reservation';
        const tourId = form.dataset.tourId || '0';
        const totalHintEmpty = form.dataset.totalHintEmpty || 'Add guests to see your estimated total.';
        const totalHintReady = form.dataset.totalHintReady || 'Proceed to payment to receive confirmation details in your inbox.';

        let currentStep = 0;

        log('init booking form');
        log('pricing data', { adultPrice, childPrice, infantPrice });

        const togglePayPalVisibility = () => {
            if (!payPalContainer) {
                return;
            }

            const selected = paymentRadios.find(r => r.checked)?.value;
            payPalContainer.style.display = selected === 'PayPal' && payPalEnabled ? '' : 'none';
            log('toggle paypal visibility', selected);
        };

        const updateTotals = () => {
            const adults = parseInput(adultInput, 1);
            const children = parseInput(childInput);
            const infants = parseInput(infantInput);
            const totalGuests = adults + children + infants;
            const total = adults * adultPrice + children * childPrice + infants * infantPrice;

            log('update totals', { adults, children, infants, totalGuests, total });

            if (totalValue) {
                totalValue.textContent = total.toLocaleString(undefined, {
                    minimumFractionDigits: 0,
                    maximumFractionDigits: 0
                });
            }

            if (totalHint) {
                totalHint.textContent = total > 0 ? totalHintReady : totalHintEmpty;
            }

            if (summaryGuests) {
                summaryGuests.textContent = totalGuests.toString();
            }

            if (summaryPrice) {
                summaryPrice.textContent = total.toLocaleString(undefined, {
                    minimumFractionDigits: 0,
                    maximumFractionDigits: 0
                });
            }

            if (payPalEnabled && payPalButton) {
                if (total > 0 && payPalEmail) {
                    const params = new URLSearchParams({
                        cmd: '_xclick',
                        business: payPalEmail,
                        currency_code: payPalCurrency,
                        amount: total.toFixed(2),
                        item_name: tourName,
                        invoice: `TA-${tourId}-${Date.now()}`
                    });

                    if (payPalReturn) params.set('return', payPalReturn);
                    if (payPalCancel) params.set('cancel_return', payPalCancel);

                    payPalButton.href = `${payPalBaseUrl}?${params.toString()}`;
                    payPalButton.classList.remove('disabled');
                    payPalButton.setAttribute('aria-disabled', 'false');
                    payPalContainer?.classList.remove('d-none');
                } else {
                    payPalButton.href = '#';
                    payPalButton.classList.add('disabled');
                    payPalButton.setAttribute('aria-disabled', 'true');
                }
            }

            togglePayPalVisibility();
        };

        const setStep = (index) => {
            currentStep = Math.max(0, Math.min(index, steps.length - 1));

            steps.forEach((step, idx) => {
                step.classList.toggle('d-none', idx !== currentStep);
            });

            progressSteps.forEach((progressStep, idx) => {
                const badge = progressStep.querySelector('.badge');
                if (!badge) return;

                if (idx < currentStep) {
                    progressStep.classList.remove('text-muted');
                    progressStep.classList.add('text-success');
                    badge.classList.remove('badge-secondary', 'badge-primary');
                    badge.classList.add('badge-success');
                } else if (idx === currentStep) {
                    progressStep.classList.remove('text-muted', 'text-success');
                    badge.classList.remove('badge-secondary', 'badge-success');
                    badge.classList.add('badge-primary');
                } else {
                    progressStep.classList.add('text-muted');
                    progressStep.classList.remove('text-success');
                    badge.classList.remove('badge-primary', 'badge-success');
                    badge.classList.add('badge-secondary');
                }
            });

            log('step changed', { currentStep });
        };

        const validateStep = (index) => {
            const step = steps[index];
            if (!step) {
                return true;
            }

            const fields = Array.from(step.querySelectorAll('input, select, textarea'))
                .filter(el => !el.disabled && el.offsetParent !== null);

            return fields.every(field => {
                const valid = field.checkValidity();
                log('validate field', { index, name: field.name, value: field.value, valid });
                if (!valid) {
                    field.reportValidity();
                }
                return valid;
            });
        };

        nextButtons.forEach(button => {
            button.addEventListener('click', () => {
                log('next clicked', { currentStep });
                if (validateStep(currentStep)) {
                    setStep(currentStep + 1);
                }
            });
        });

        prevButtons.forEach(button => {
            button.addEventListener('click', () => {
                log('back clicked', { currentStep });
                setStep(currentStep - 1);
            });
        });

        guestInputs.forEach(input => {
            input.addEventListener('input', updateTotals);
            input.addEventListener('change', updateTotals);
            input.addEventListener('blur', updateTotals);
        });

        paymentRadios.forEach(radio => radio.addEventListener('change', togglePayPalVisibility));
        form.addEventListener('submit', updateTotals);

        setStep(0);
        updateTotals();
        log('initial totals calculated');
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
