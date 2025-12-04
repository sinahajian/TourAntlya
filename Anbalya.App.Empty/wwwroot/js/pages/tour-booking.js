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

        const phoneInput = form.querySelector('[data-phone-input]');
        const phoneMenu = form.querySelector('[data-phone-country-menu]');
        const phoneToggle = form.querySelector('.phone-country-toggle');
        const phoneFlag = phoneToggle?.querySelector('[data-phone-flag]');
        const phoneCode = phoneToggle?.querySelector('[data-phone-code]');
        let phoneMenuSearchInput = null;
        let phoneMenuOpen = false;

        const phoneCountries = [
            { code: 'TR', name: 'Turkey', dial: '+90', flag: '🇹🇷', example: '+90 555 123 4567' },
            { code: 'IR', name: 'Iran', dial: '+98', flag: '🇮🇷', example: '+98 912 123 4567' },
            { code: 'AZ', name: 'Azerbaijan', dial: '+994', flag: '🇦🇿', example: '+994 50 123 45 67' },
            { code: 'AF', name: 'Afghanistan', dial: '+93', flag: '🇦🇫', example: '+93 70 123 4567' },
            { code: 'IQ', name: 'Iraq', dial: '+964', flag: '🇮🇶', example: '+964 750 123 4567' },
            { code: 'SA', name: 'Saudi Arabia', dial: '+966', flag: '🇸🇦', example: '+966 50 123 4567' },
            { code: 'AE', name: 'United Arab Emirates', dial: '+971', flag: '🇦🇪', example: '+971 50 123 4567' },
            { code: 'QA', name: 'Qatar', dial: '+974', flag: '🇶🇦', example: '+974 5012 3456' },
            { code: 'OM', name: 'Oman', dial: '+968', flag: '🇴🇲', example: '+968 90 123456' },
            { code: 'KW', name: 'Kuwait', dial: '+965', flag: '🇰🇼', example: '+965 5012 3456' },
            { code: 'BH', name: 'Bahrain', dial: '+973', flag: '🇧🇭', example: '+973 3212 3456' },
            { code: 'LB', name: 'Lebanon', dial: '+961', flag: '🇱🇧', example: '+961 71 234 567' },
            { code: 'JO', name: 'Jordan', dial: '+962', flag: '🇯🇴', example: '+962 7 9012 3456' },
            { code: 'SY', name: 'Syria', dial: '+963', flag: '🇸🇾', example: '+963 944 123 456' },
            { code: 'EG', name: 'Egypt', dial: '+20', flag: '🇪🇬', example: '+20 100 123 4567' },
            { code: 'MA', name: 'Morocco', dial: '+212', flag: '🇲🇦', example: '+212 600 123456' },
            { code: 'DZ', name: 'Algeria', dial: '+213', flag: '🇩🇿', example: '+213 551 23 45 67' },
            { code: 'TN', name: 'Tunisia', dial: '+216', flag: '🇹🇳', example: '+216 20 123 456' },
            { code: 'RU', name: 'Russia', dial: '+7', flag: '🇷🇺', example: '+7 912 123 45 67' },
            { code: 'UA', name: 'Ukraine', dial: '+380', flag: '🇺🇦', example: '+380 67 123 4567' },
            { code: 'GE', name: 'Georgia', dial: '+995', flag: '🇬🇪', example: '+995 555 12 34 56' },
            { code: 'AM', name: 'Armenia', dial: '+374', flag: '🇦🇲', example: '+374 91 234567' },
            { code: 'KZ', name: 'Kazakhstan', dial: '+7', flag: '🇰🇿', example: '+7 701 123 4567' },
            { code: 'DE', name: 'Germany', dial: '+49', flag: '🇩🇪', example: '+49 1512 3456789' },
            { code: 'FR', name: 'France', dial: '+33', flag: '🇫🇷', example: '+33 6 12 34 56 78' },
            { code: 'IT', name: 'Italy', dial: '+39', flag: '🇮🇹', example: '+39 345 123 4567' },
            { code: 'ES', name: 'Spain', dial: '+34', flag: '🇪🇸', example: '+34 612 345 678' },
            { code: 'PT', name: 'Portugal', dial: '+351', flag: '🇵🇹', example: '+351 912 345 678' },
            { code: 'GB', name: 'United Kingdom', dial: '+44', flag: '🇬🇧', example: '+44 7700 900123' },
            { code: 'IE', name: 'Ireland', dial: '+353', flag: '🇮🇪', example: '+353 85 123 4567' },
            { code: 'NL', name: 'Netherlands', dial: '+31', flag: '🇳🇱', example: '+31 6 12345678' },
            { code: 'BE', name: 'Belgium', dial: '+32', flag: '🇧🇪', example: '+32 470 12 34 56' },
            { code: 'PL', name: 'Poland', dial: '+48', flag: '🇵🇱', example: '+48 600 123 456' },
            { code: 'SE', name: 'Sweden', dial: '+46', flag: '🇸🇪', example: '+46 70 123 45 67' },
            { code: 'NO', name: 'Norway', dial: '+47', flag: '🇳🇴', example: '+47 412 34 567' },
            { code: 'DK', name: 'Denmark', dial: '+45', flag: '🇩🇰', example: '+45 20 12 34 56' },
            { code: 'FI', name: 'Finland', dial: '+358', flag: '🇫🇮', example: '+358 40 1234567' },
            { code: 'CH', name: 'Switzerland', dial: '+41', flag: '🇨🇭', example: '+41 79 123 45 67' },
            { code: 'AT', name: 'Austria', dial: '+43', flag: '🇦🇹', example: '+43 660 1234567' },
            { code: 'GR', name: 'Greece', dial: '+30', flag: '🇬🇷', example: '+30 691 234 5678' },
            { code: 'CY', name: 'Cyprus', dial: '+357', flag: '🇨🇾', example: '+357 96 123456' },
            { code: 'BG', name: 'Bulgaria', dial: '+359', flag: '🇧🇬', example: '+359 88 123 4567' },
            { code: 'RO', name: 'Romania', dial: '+40', flag: '🇷🇴', example: '+40 712 345 678' },
            { code: 'HU', name: 'Hungary', dial: '+36', flag: '🇭🇺', example: '+36 20 123 4567' },
            { code: 'CZ', name: 'Czech Republic', dial: '+420', flag: '🇨🇿', example: '+420 601 123 456' },
            { code: 'SK', name: 'Slovakia', dial: '+421', flag: '🇸🇰', example: '+421 902 123 456' },
            { code: 'HR', name: 'Croatia', dial: '+385', flag: '🇭🇷', example: '+385 91 123 4567' },
            { code: 'BA', name: 'Bosnia & Herzegovina', dial: '+387', flag: '🇧🇦', example: '+387 61 123 456' },
            { code: 'RS', name: 'Serbia', dial: '+381', flag: '🇷🇸', example: '+381 60 123 4567' },
            { code: 'MK', name: 'North Macedonia', dial: '+389', flag: '🇲🇰', example: '+389 70 123 456' },
            { code: 'AL', name: 'Albania', dial: '+355', flag: '🇦🇱', example: '+355 68 20 12345' },
            { code: 'US', name: 'United States', dial: '+1', flag: '🇺🇸', example: '+1 415 555 2671' },
            { code: 'CA', name: 'Canada', dial: '+1', flag: '🇨🇦', example: '+1 647 123 4567' },
            { code: 'MX', name: 'Mexico', dial: '+52', flag: '🇲🇽', example: '+52 55 1234 5678' },
            { code: 'BR', name: 'Brazil', dial: '+55', flag: '🇧🇷', example: '+55 11 91234 5678' },
            { code: 'AR', name: 'Argentina', dial: '+54', flag: '🇦🇷', example: '+54 9 11 2345 6789' },
            { code: 'CO', name: 'Colombia', dial: '+57', flag: '🇨🇴', example: '+57 310 1234567' },
            { code: 'CL', name: 'Chile', dial: '+56', flag: '🇨🇱', example: '+56 9 6123 4567' },
            { code: 'PE', name: 'Peru', dial: '+51', flag: '🇵🇪', example: '+51 912 345 678' },
            { code: 'UY', name: 'Uruguay', dial: '+598', flag: '🇺🇾', example: '+598 91 234 567' },
            { code: 'EC', name: 'Ecuador', dial: '+593', flag: '🇪🇨', example: '+593 99 123 4567' },
            { code: 'VE', name: 'Venezuela', dial: '+58', flag: '🇻🇪', example: '+58 412 1234567' },
            { code: 'AU', name: 'Australia', dial: '+61', flag: '🇦🇺', example: '+61 412 345 678' },
            { code: 'NZ', name: 'New Zealand', dial: '+64', flag: '🇳🇿', example: '+64 21 123 4567' },
            { code: 'CN', name: 'China', dial: '+86', flag: '🇨🇳', example: '+86 131 2345 6789' },
            { code: 'HK', name: 'Hong Kong', dial: '+852', flag: '🇭🇰', example: '+852 5123 4567' },
            { code: 'JP', name: 'Japan', dial: '+81', flag: '🇯🇵', example: '+81 90 1234 5678' },
            { code: 'KR', name: 'South Korea', dial: '+82', flag: '🇰🇷', example: '+82 10 1234 5678' },
            { code: 'SG', name: 'Singapore', dial: '+65', flag: '🇸🇬', example: '+65 8123 4567' },
            { code: 'MY', name: 'Malaysia', dial: '+60', flag: '🇲🇾', example: '+60 12 345 6789' },
            { code: 'TH', name: 'Thailand', dial: '+66', flag: '🇹🇭', example: '+66 81 234 5678' },
            { code: 'VN', name: 'Vietnam', dial: '+84', flag: '🇻🇳', example: '+84 91 234 5678' },
            { code: 'PH', name: 'Philippines', dial: '+63', flag: '🇵🇭', example: '+63 912 345 6789' },
            { code: 'ID', name: 'Indonesia', dial: '+62', flag: '🇮🇩', example: '+62 812 3456 7890' },
            { code: 'IN', name: 'India', dial: '+91', flag: '🇮🇳', example: '+91 98765 43210' },
            { code: 'PK', name: 'Pakistan', dial: '+92', flag: '🇵🇰', example: '+92 300 1234567' },
            { code: 'BD', name: 'Bangladesh', dial: '+880', flag: '🇧🇩', example: '+880 1712 345678' },
            { code: 'LK', name: 'Sri Lanka', dial: '+94', flag: '🇱🇰', example: '+94 71 234 5678' },
            { code: 'NP', name: 'Nepal', dial: '+977', flag: '🇳🇵', example: '+977 981 234 5678' },
            { code: 'KE', name: 'Kenya', dial: '+254', flag: '🇰🇪', example: '+254 712 345678' },
            { code: 'TZ', name: 'Tanzania', dial: '+255', flag: '🇹🇿', example: '+255 712 345 678' },
            { code: 'UG', name: 'Uganda', dial: '+256', flag: '🇺🇬', example: '+256 712 345678' },
            { code: 'NG', name: 'Nigeria', dial: '+234', flag: '🇳🇬', example: '+234 803 123 4567' },
            { code: 'GH', name: 'Ghana', dial: '+233', flag: '🇬🇭', example: '+233 20 123 4567' },
            { code: 'ZA', name: 'South Africa', dial: '+27', flag: '🇿🇦', example: '+27 82 123 4567' },
            { code: 'ET', name: 'Ethiopia', dial: '+251', flag: '🇪🇹', example: '+251 91 123 4567' },
            { code: 'CM', name: 'Cameroon', dial: '+237', flag: '🇨🇲', example: '+237 6 71 23 45 67' },
            { code: 'SN', name: 'Senegal', dial: '+221', flag: '🇸🇳', example: '+221 70 123 4567' }
        ];
        const fallbackPhoneCountry = { code: 'OTHER', name: 'Other', dial: '', flag: '🌐', example: 'e.g. +123 456 7890' };
        const defaultPhoneCountry = phoneCountries[0];

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
        const paymentReferenceInput = form.querySelector('input[name="Form.PaymentReference"]');
        const totalHintEmpty = form.dataset.totalHintEmpty || 'Add guests to see your estimated total.';
        const totalHintReady = form.dataset.totalHintReady || 'Proceed to payment to receive confirmation details in your inbox.';

        let currentStep = 0;

        log('init booking form');
        log('pricing data', { adultPrice, childPrice, infantPrice });

        const ensureInvoice = () => {
            const currentTotal = Number(form.dataset.currentTotal || 0);
            if (!payPalEnabled || currentTotal <= 0) {
                form.dataset.paypalInvoice = '';
                if (paymentReferenceInput) paymentReferenceInput.value = '';
                return null;
            }

            let invoice = form.dataset.paypalInvoice || paymentReferenceInput?.value;

            if (!invoice) {
                invoice = `TA-${tourId}-${Date.now()}`;
                form.dataset.paypalInvoice = invoice;
            }

            if (paymentReferenceInput && paymentReferenceInput.value !== invoice) {
                paymentReferenceInput.value = invoice;
            }

            return invoice;
        };

        const togglePayPalVisibility = () => {
            if (!payPalContainer) {
                return;
            }

            const selected = paymentRadios.find(r => r.checked)?.value;
            const currentTotal = Number(form.dataset.currentTotal || 0);
            const visible = selected === 'PayPal' && payPalEnabled && currentTotal > 0;
            payPalContainer.style.display = visible ? '' : 'none';
            log('toggle paypal visibility', selected);
        };

        const isPayPalSelectedAndReady = () => {
            const selected = paymentRadios.find(r => r.checked)?.value;
            const currentTotal = Number(form.dataset.currentTotal || 0);
            return selected === 'PayPal' && payPalEnabled && currentTotal > 0 && payPalButton && !payPalButton.classList.contains('disabled') && payPalButton.href && payPalButton.href !== '#';
        };

        const updateTotals = () => {
            const adults = parseInput(adultInput, 1);
            const children = parseInput(childInput);
            const infants = parseInput(infantInput);
            const totalGuests = adults + children + infants;
            const total = adults * adultPrice + children * childPrice + infants * infantPrice;
            form.dataset.currentTotal = String(total);

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
                    const invoiceId = ensureInvoice();
                    const params = new URLSearchParams({
                        cmd: '_xclick',
                        business: payPalEmail,
                        currency_code: payPalCurrency,
                        amount: total.toFixed(2),
                        item_name: tourName,
                        invoice: invoiceId
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
                    form.dataset.paypalInvoice = '';
                    if (paymentReferenceInput) {
                        paymentReferenceInput.value = '';
                    }
                    if (payPalContainer) {
                        payPalContainer.style.display = 'none';
                    }
                }
            }

            togglePayPalVisibility();
        };

        const normalizeDial = (dial = '') => dial.replace(/[^\d+]/g, '');

        const findCountryByDial = (value = '') => {
            const normalizedValue = value.replace(/\s+/g, '');
            const sorted = [...phoneCountries].sort((a, b) => normalizeDial(b.dial).length - normalizeDial(a.dial).length);
            const found = sorted.find(country => normalizedValue.startsWith(normalizeDial(country.dial)));
            if (found) return found;
            const dialGuessMatch = normalizedValue.match(/^\+?\d{1,4}/);
            if (dialGuessMatch) {
                return { ...fallbackPhoneCountry, dial: dialGuessMatch[0].startsWith('+') ? dialGuessMatch[0] : `+${dialGuessMatch[0]}` };
            }
            return fallbackPhoneCountry;
        };

        const findCountryByCode = (code = '') => phoneCountries.find(country => country.code === code) || null;

        const stripKnownDialCode = (value = '') => {
            const match = findCountryByDial(value);
            if (match) {
                return value.trim().slice(match.dial.length).trim();
            }
            return value.trim().replace(/^\+?\d{1,4}\s*/, '');
        };

        const stripSpecificDial = (value = '', dial = '') => {
            const trimmedValue = (value || '').trim();
            if (dial && trimmedValue.startsWith(dial)) {
                return trimmedValue.slice(dial.length).trim();
            }
            return stripKnownDialCode(trimmedValue);
        };

        const combinePhoneValue = () => {
            if (!phoneInput) return;
            const dial = (phoneCode?.textContent || '').trim();
            const local = stripKnownDialCode(phoneInput.value || '');
            const combined = dial ? `${dial} ${local}`.trim() : local;
            phoneInput.value = combined;
        };

        const formatLocalNumber = (value = '') => {
            const digits = (value || '').replace(/[^\d]/g, '');
            if (!digits) return '';
            const groups = [3, 2, 2];
            const parts = [];
            let index = 0;
            let groupIndex = 0;

            while (index < digits.length) {
                const size = groupIndex < groups.length ? groups[groupIndex] : 2;
                parts.push(digits.slice(index, index + size));
                index += size;
                groupIndex += 1;
            }

            return parts.join(' ');
        };

        const handleLocalFormat = () => {
            if (!phoneInput) return;
            phoneInput.value = formatLocalNumber(phoneInput.value);
        };

        const filterPhoneMenu = (query = '') => {
            if (!phoneMenu) return;
            const normalized = query.trim().toLowerCase();
            const items = phoneMenu.querySelectorAll('[data-country-search]');
            items.forEach(item => {
                const haystack = item.dataset.countrySearch || '';
                const match = !normalized || haystack.includes(normalized);
                item.classList.toggle('d-none', !match);
            });
        };

        const updatePhonePlaceholder = (country) => {
            if (!phoneInput) {
                return;
            }
            const example = country?.example || '';
            const dial = country?.dial || '';
            let localExample = example;
            if (example && dial && example.trim().startsWith(dial)) {
                localExample = example.trim().slice(dial.length).trim();
            }
            phoneInput.placeholder = localExample || 'WhatsApp or mobile number';
        };

        const setPhoneCountry = (country, options = {}) => {
            if (!country || !phoneToggle) {
                return;
            }

            const { replaceValue = false } = options;

            phoneToggle.dataset.selectedCountry = country.code;
            if (phoneFlag) phoneFlag.textContent = country.flag;
            if (phoneCode) phoneCode.textContent = country.dial || '+';

            if (replaceValue && phoneInput) {
                const remainder = stripSpecificDial(phoneInput.value || '', country.dial);
                phoneInput.value = remainder;
                phoneInput.focus();
            }

            updatePhonePlaceholder(country);
        };

        const buildPhoneMenu = () => {
            if (!phoneMenu) return;
            phoneMenu.innerHTML = '';
            phoneMenuSearchInput = document.createElement('input');
            phoneMenuSearchInput.type = 'text';
            phoneMenuSearchInput.className = 'form-control phone-menu-search-input';
            phoneMenuSearchInput.placeholder = 'Search country or code';
            phoneMenuSearchInput.addEventListener('input', (event) => filterPhoneMenu(event.target.value));

            const searchWrapper = document.createElement('div');
            searchWrapper.className = 'phone-menu-search';
            searchWrapper.appendChild(phoneMenuSearchInput);
            phoneMenu.appendChild(searchWrapper);

            const listWrapper = document.createElement('div');
            listWrapper.className = 'phone-menu-items';
            phoneMenu.appendChild(listWrapper);

            phoneCountries.forEach(country => {
                const item = document.createElement('button');
                item.type = 'button';
                item.className = 'dropdown-item';
                item.dataset.countryCode = country.code;
                item.dataset.dial = country.dial;
                item.dataset.countrySearch = `${country.name} ${country.dial} ${country.code}`.toLowerCase();
                item.innerHTML = `
                    <span class="phone-flag mr-2">${country.flag}</span>
                    <span class="phone-country-code mr-2">${country.dial}</span>
                    <span class="phone-country-name">${country.name}</span>
                `;
                item.addEventListener('click', () => {
                    setPhoneCountry(country, { replaceValue: false });
                    closePhoneMenu();
                });
                listWrapper.appendChild(item);
            });
        };

        const openPhoneMenu = () => {
            if (!phoneMenu) return;
            phoneMenu.classList.add('show');
            phoneMenuOpen = true;
            if (phoneMenuSearchInput) {
                phoneMenuSearchInput.focus();
                phoneMenuSearchInput.select();
            }
        };

        const closePhoneMenu = () => {
            if (!phoneMenu) return;
            phoneMenu.classList.remove('show');
            phoneMenuOpen = false;
        };

        const togglePhoneMenu = () => {
            if (phoneMenuOpen) {
                closePhoneMenu();
            } else {
                openPhoneMenu();
            }
        };

        const syncPhoneFromInput = () => {
            if (!phoneInput) return;
            const value = phoneInput.value || '';
            const match = findCountryByDial(value) || defaultPhoneCountry;
            setPhoneCountry(match);
            if (match && phoneInput) {
                phoneInput.value = stripSpecificDial(value, match.dial);
            }
        };

        const hydratePhoneInput = () => {
            buildPhoneMenu();
            if (!phoneInput) return;
            setPhoneCountry(defaultPhoneCountry);
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

        form.addEventListener('submit', () => {
            ensureInvoice();
            combinePhoneValue();
            if (isPayPalSelectedAndReady()) {
                window.open(payPalButton.href, '_blank', 'noopener');
            }
        });

        paymentRadios.forEach(radio => radio.addEventListener('change', togglePayPalVisibility));
        form.addEventListener('submit', updateTotals);

        if (phoneInput && phoneToggle && phoneMenu) {
            hydratePhoneInput();
            updatePhonePlaceholder(defaultPhoneCountry);
            phoneInput.addEventListener('input', handleLocalFormat);
            phoneInput.addEventListener('blur', handleLocalFormat);
            phoneInput.addEventListener('focus', () => {
                const selected = findCountryByCode(phoneToggle.dataset.selectedCountry) || defaultPhoneCountry;
                if (!phoneInput.value.trim()) {
                    setPhoneCountry(selected, { replaceValue: true });
                }
                closePhoneMenu();
            });
            phoneToggle.addEventListener('click', (event) => {
                event.preventDefault();
                togglePhoneMenu();
            });
            if (phoneMenuSearchInput) {
                phoneMenuSearchInput.addEventListener('click', (event) => event.stopPropagation());
            }
            document.addEventListener('click', (event) => {
                const target = event.target;
                if (!phoneMenuOpen) return;
                if (!phoneMenu.contains(target) && !phoneToggle.contains(target)) {
                    closePhoneMenu();
                }
            });
            document.addEventListener('keydown', (event) => {
                if (event.key === 'Escape') {
                    closePhoneMenu();
                }
            });
        }

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
