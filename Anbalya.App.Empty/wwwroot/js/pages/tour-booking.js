(() => {
    const log = (...args) => console.log('[tour-booking]', ...args);

    const init = () => {
        const form = document.querySelector('.booking-form');
        if (!form) {
            log('booking form not found');
            return;
        }

        log('init booking form');

        const totalValue = document.getElementById('reservation-total-value');
        const totalHint = document.getElementById('reservation-total-hint');
        const adultInput = form.querySelector('.js-booking-adults');
        const childInput = form.querySelector('.js-booking-children');
        const infantInput = form.querySelector('.js-booking-infants');
        const summaryGuests = document.getElementById('summary-total-guests');
        const summaryPrice = document.getElementById('summary-total-price');
        const payPalButton = document.getElementById('paypalCheckoutButton');
        const payPalContainer = document.getElementById('paypalCheckoutContainer');
        const paymentRadios = Array.from(form.querySelectorAll('input[name="Form.PaymentMethod"]'));

        const adultPrice = Number(form.dataset.adultPrice || 0);
        const childPrice = Number(form.dataset.childPrice || 0);
        const infantPrice = Number(form.dataset.infantPrice || 0);
        const totalHintEmpty = form.dataset.totalHintEmpty || 'Add guests to see your estimated total.';
        const totalHintReady = form.dataset.totalHintReady || 'Proceed to payment to receive confirmation details in your inbox.';
        const payPalEnabled = form.dataset.paypalEnabled === 'true';
        const payPalBaseUrl = form.dataset.paypalBaseUrl || '';
        const payPalEmail = form.dataset.paypalEmail || '';
        const payPalCurrency = form.dataset.paypalCurrency || 'EUR';
        const payPalReturn = form.dataset.paypalReturn || '';
        const payPalCancel = form.dataset.paypalCancel || '';
        const tourName = form.dataset.tourName || 'Tour Reservation';
        const tourId = form.dataset.tourId || '0';

        log('pricing data', { adultPrice, childPrice, infantPrice });

        const parseInput = (input) => {
            if (!input) {
                return 0;
            }

            const value = Number(input.value);
            return Number.isFinite(value) ? Math.max(0, Math.floor(value)) : 0;
        };

        const togglePayPalVisibility = () => {
            if (!payPalContainer) {
                return;
            }

            const selected = paymentRadios.find(r => r.checked)?.value;
            payPalContainer.style.display = selected === 'PayPal' && payPalEnabled ? '' : 'none';
            log('toggle paypal visibility', selected);
        };

        const updateTotals = () => {
            const adults = parseInput(adultInput);
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

                    if (payPalReturn) {
                        params.set('return', payPalReturn);
                    }
                    if (payPalCancel) {
                        params.set('cancel_return', payPalCancel);
                    }

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

        const guestInputs = Array.from(form.querySelectorAll('.js-booking-input'));
        guestInputs.forEach(input => {
            if (!input) return;

            const handleUpdate = () => {
                const value = parseInput(input);
                const label = input.previousElementSibling?.textContent?.trim() || 'Guests';
                log('guest input changed', label, value);
                updateTotals();
            };

            input.addEventListener('input', handleUpdate);
            input.addEventListener('change', handleUpdate);
            input.addEventListener('blur', handleUpdate);
        });

        paymentRadios.forEach(radio => radio.addEventListener('change', togglePayPalVisibility));
        form.addEventListener('submit', updateTotals);

        updateTotals();
        log('initial totals calculated');
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
